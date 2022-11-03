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

import com.alttester.altTesterExceptions.*;

@ClientEndpoint
public class WebsocketConnection {
    private static final Logger logger = LogManager.getLogger(AltDriver.class);
    private String _uri;
    private String _host;
    private int _port;
    private String _gameName;
    private int _connectTimeout;

    public Session session = null;
    public IMessageHandler messageHandler = null;

    public WebsocketConnection(String host, int port, int connectTimeout, String gameName) {
        _host = host;
        _port = port;
        _gameName = gameName;
        _connectTimeout = connectTimeout;
    }

    public URI getURI() throws ConnectionException {
        try {
            return new URI("ws", null, _host, _port, "/altws", "game=" + _gameName, null);
        } catch (URISyntaxException e) {
            logger.error(e);
            throw new ConnectionException(e.getMessage(), e);
        }
    }

    public void connect() {
        URI uri = getURI();
        logger.info("Connecting to: '{}'.", uri.toString());

        int delay = 100;
        int retries = 0;
        long timeout = _connectTimeout * 1000;
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
                Thread.sleep(delay); // delay between retries
            } catch (InterruptedException e) {
                break;
            }

            if (this.session != null && this.session.isOpen()) {
                break;
            }

            retries++;
            finish = System.currentTimeMillis();
        }

        if (this.session == null || (!this.session.isOpen() && finish - start >= timeout)) {
            throw new ConnectionTimeoutException(
                    String.format("Failed to connect to AltTester on host: %s port: %s.", _host, _port),
                    connectionError);
        }

        if (!this.session.isOpen()) {
            throw new ConnectionException(
                    String.format("Failed to connect to AltTester on host: %s port: %s.", _host, _port),
                    connectionError);
        }
    }

    public void close() throws IOException {
        logger.info(String.format("Closing connection to AltTester on host: %s port: %s.", _host, _port));

        if (this.session != null) {
            this.session.close();
        }
    }

    @OnOpen
    public void onOpen(Session session) {
        logger.debug("Connected to: " + session.getRequestURI().toString());
        this.session = session;
        this.messageHandler = new MessageHandler(session);
    }

    @OnMessage
    public void onMessage(String message) {
        messageHandler.onMessage(message);
    }

    // Processing when receiving a message
    @OnError
    public void onError(Throwable th) {
        logger.error(th.getMessage());
        logger.error(th);
    }

    // Processing at session release
    @OnClose
    public void onClose(Session session, CloseReason reason) {
        logger.debug("Connection to AltTester closed: {}.", reason.toString());
    }
}
