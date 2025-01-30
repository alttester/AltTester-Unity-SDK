/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.net.URI;
import java.net.URISyntaxException;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;

import javax.websocket.ClientEndpoint;
import javax.websocket.CloseReason;
import javax.websocket.ContainerProvider;
import javax.websocket.DeploymentException;
import javax.websocket.Session;
import javax.websocket.WebSocketContainer;
import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import com.alttester.altTesterExceptions.ConnectionException;
import com.alttester.altTesterExceptions.ConnectionTimeoutException;
import com.alttester.altTesterExceptions.NoAppConnectedException;
import com.alttester.altTesterExceptions.AppDisconnectedException;
import com.alttester.altTesterExceptions.MultipleDriversException;
import com.alttester.altTesterExceptions.MultipleDriversTryingToConnectException;

@ClientEndpoint
public class WebsocketConnection {
    private static final Logger logger = LogManager.getLogger(AltDriver.class);

    private String host;
    private int port;
    private String appName;
    private int connectTimeout;
    private String platform;
    private String platformVersion;
    private String deviceInstanceId;
    private String appId;

    private String error = null;
    private CloseReason closeReason = null;

    public Session session = null;
    public IMessageHandler messageHandler = null;
    public boolean driverRegisteredCalled = false;

    public WebsocketConnection(String host, int port, String appName, int connectTimeout, String platform,
            String platformVersion, String deviceInstanceId, String appId) {
        this.host = host;
        this.port = port;
        this.appName = appName;
        this.connectTimeout = connectTimeout;
        this.platform = platform;
        this.platformVersion = platformVersion;
        this.deviceInstanceId = deviceInstanceId;
        this.appId = appId;
    }

    public boolean isOpen() {
        return this.session.isOpen();
    }

    public static String escapeDataString(String value) {
        try {
            return URLEncoder.encode(value, StandardCharsets.UTF_8.toString());
        } catch (UnsupportedEncodingException e) {
            return null;
        }
    }

    private URI getURI() {

        String query = String.format(
                "appName=%s&platform=%s&platformVersion=%s&deviceInstanceId=%s&appId=%s&driverType=SDK",
                escapeDataString(appName), escapeDataString(platform),
                escapeDataString(platformVersion), escapeDataString(deviceInstanceId),
                escapeDataString(appId));

        try {
            return new URI("ws", null, host, port, "/altws", query, null);
        } catch (URISyntaxException e) {
            logger.error(e);
            throw new ConnectionException(e.getMessage(), e);
        }
    }

    private void checkErrors() {
        if (this.error != null) {
            throw new ConnectionException(this.error);
        }
    }

    private void checkCloseReason() {
        if (closeReason != null) {
            int code = closeReason.getCloseCode().getCode();
            String reason = closeReason.getReasonPhrase();

            if (code == 4001) {
                throw new NoAppConnectedException(reason);
            }

            if (code == 4002) {
                throw new AppDisconnectedException(reason);
            }

            if (code == 4005) {
                throw new MultipleDriversException(reason);
            }
            if (code == 4007) {
                throw new MultipleDriversTryingToConnectException(reason);
            }
            throw new ConnectionException(
                    String.format("Connection closed by AltTester(R) Server with reason: %s.", reason));
        }
    }

    private void ensureConnectionIsOpen() {
        this.checkCloseReason();
        this.checkErrors();

        if (!this.isOpen()) {
            throw new ConnectionException("Connection closed. An unexpected error ocurred.");
        }
    }

    public void connect() {
        URI uri = getURI();
        logger.info("Connecting to: '{}'.", uri.toString());

        int delay = 100;
        int retries = 0;
        long timeout = connectTimeout * 1000;
        long start = System.currentTimeMillis();
        long finish = System.currentTimeMillis();

        WebSocketContainer container = ContainerProvider.getWebSocketContainer();

        Exception connectionError = null;

        while (finish - start < timeout) {
            this.error = null;
            this.closeReason = null;

            try {
                if (retries > 0) {
                    logger.debug("Retrying #{} to: '{}'.", retries, uri);
                }

                this.session = container.connectToServer(this, uri);
            } catch (IllegalStateException e) {
                logger.error(e);
                throw new ConnectionException(e.getMessage(), e);
            } catch (DeploymentException | IOException e) {
                connectionError = e;
            }

            float waitForNotification = 0;
            try {

                while (waitForNotification < 5000) {
                    if (driverRegisteredCalled) {
                        logger.debug("Connected to: '{}'.", uri);
                        return;
                    }
                    try {
                        Thread.sleep(delay);
                    } catch (InterruptedException e) {
                        e.printStackTrace();
                    }
                    checkCloseReason();

                    waitForNotification += delay;
                }
            } catch (Exception e) {

            }

            if (session.isOpen()) {// Added this to be also backward compatible but it will be slower
                break;
            }

            retries++;
            finish = System.currentTimeMillis();
        }

        this.checkCloseReason();
        this.checkErrors();

        if (this.session == null || (!this.session.isOpen() && finish - start >= timeout)) {
            throw new ConnectionTimeoutException(
                    String.format("Failed to connect to AltTester(R) on host: %s port: %s.", host, port),
                    connectionError);
        }

        if (!this.session.isOpen()) {
            throw new ConnectionException(
                    String.format("Failed to connect to AltTester(R) on host: %s port: %s.", host, port),
                    connectionError);
        }
    }

    public void close() {
        logger.info("Closing connection to AltTester(R) on host: {} port: {}.", host, port);

        if (this.session != null) {
            try {
                this.session.close();
            } catch (Exception e) {
                throw new ConnectionException("An unexpected error occurred while closing the connection.", e);
            }
        }
    }

    public void send(String message) {
        this.ensureConnectionIsOpen();

        if (this.session != null) {
            session.getAsyncRemote().sendText(message);
        }
    }

    @OnOpen
    public void onOpen(Session session) {
        logger.debug("Connected to: {}.", session.getRequestURI().toString());

        this.session = session;
        this.messageHandler = new MessageHandler(this);
    }

    @OnMessage
    public void onMessage(String message) {
        if (message.contains("driverRegistered")) {
            driverRegisteredCalled = true;
        } else {
            messageHandler.onMessage(message);
        }
    }

    @OnError
    public void onError(Throwable th) {
        logger.error(th.getMessage());
        logger.error(th);

        this.error = th.getMessage();
    }

    @OnClose
    public void onClose(Session session, CloseReason reason) {
        logger.debug("Connection to AltTester(R) closed: {}.", reason.toString());

        this.closeReason = reason;
    }
}
