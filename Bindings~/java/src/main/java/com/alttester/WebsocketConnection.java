package com.alttester;

import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

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


@ClientEndpoint
public class WebsocketConnection {
    private static final Logger logger = LogManager.getLogger(AltDriver.class);

    private String host;
    private int port;
    private String appName;
    private int connectTimeout;

    private String error = null;
    private CloseReason closeReason = null;

    public Session session = null;
    public IMessageHandler messageHandler = null;

    public WebsocketConnection(String host, int port, String appName, int connectTimeout) {
        this.host = host;
        this.port = port;
        this.appName = appName;
        this.connectTimeout = connectTimeout;
    }

    public boolean isOpen() {
        return this.session.isOpen();
    }

    private URI getURI() {
        try {
            return new URI("ws", null, host, port, "/altws", "appName=" + appName, null);
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

            throw new ConnectionException(String.format("Connection closed by AltServer with reason: %s.", reason));
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

            try {
                Thread.sleep(delay); // Delay between retries.
            } catch (InterruptedException e) {
                break;
            }

            if (this.session != null && this.session.isOpen()) {
                break;
            }

            retries++;
            finish = System.currentTimeMillis();
        }

        this.checkCloseReason();
        this.checkErrors();

        if (this.session == null || (!this.session.isOpen() && finish - start >= timeout)) {
            throw new ConnectionTimeoutException(
                    String.format("Failed to connect to AltTester on host: %s port: %s.", host, port),
                    connectionError);
        }

        if (!this.session.isOpen()) {
            throw new ConnectionException(
                    String.format("Failed to connect to AltTester on host: %s port: %s.", host, port),
                    connectionError);
        }
    }

    public void close() {
        logger.info("Closing connection to AltTester on host: {} port: {}.", host, port);

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
        messageHandler.onMessage(message);
    }

    @OnError
    public void onError(Throwable th) {
        logger.error(th.getMessage());
        logger.error(th);

        this.error = th.getMessage();
    }

    @OnClose
    public void onClose(Session session, CloseReason reason) {
        logger.debug("Connection to AltTester closed: {}.", reason.toString());

        this.closeReason = reason;
    }
}
