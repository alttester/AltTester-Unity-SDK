package ro.altom.altunitytester;

import java.io.IOException;

import javax.websocket.ClientEndpoint;
import javax.websocket.CloseReason;
import javax.websocket.ContainerProvider;
import javax.websocket.DeploymentException;
import javax.websocket.Session;
import javax.websocket.WebSocketContainer;
import java.net.URI;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;

import ro.altom.altunitytester.altUnityTesterExceptions.*;

@ClientEndpoint
public class WebsocketConnection {
    private static final Logger logger = LogManager.getLogger(AltUnityDriver.class);
    private String _uri;
    private String _host;
    private int _port;
    private int _connectTimeout;
    public Session session = null;
    public IMessageHandler messageHandler = null;

    @OnOpen
    public void onOpen(Session session) {
        logger.debug("Connected to: " + _uri);
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
        logger.debug("Connection to AltUnity closed: {}.", reason.toString());
    }

    public WebsocketConnection(String host, int port, int connectTimeout) {
        _host = host;
        _port = port;
        _uri = "ws://" + host + ":" + port + "/altws/";
        _connectTimeout = connectTimeout;
    }

    public void connect() {
        int delay = 100;
        logger.info("Connecting to host: {} port: {}.", _host, _port);
        long start = System.currentTimeMillis();
        long finish = System.currentTimeMillis();
        int retries = 0;

        WebSocketContainer container = ContainerProvider.getWebSocketContainer();

        Exception connectionError = null;
        while (finish - start < _connectTimeout * 1000) {
            try {
                if (retries > 0) {
                    logger.debug("Retrying #{} to host: {} port: {}.", retries, _host, _port);
                }

                this.session = container.connectToServer(this, URI.create(_uri));

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

            if (this.session != null && this.session.isOpen())
                break;

            retries++;
            finish = System.currentTimeMillis();
        }
        if (this.session == null || (!this.session.isOpen() && finish - start >= _connectTimeout * 1000)) {
            throw new ConnectionTimeoutException(
                    String.format("Failed to connect to AltUnity Tester on host: %s port: %s.", _host, _port),
                    connectionError);
        }
        if (!this.session.isOpen())
            throw new ConnectionException(
                    String.format("Failed to connect to AltUnity Tester on host: %s port: %s.", _host, _port),
                    connectionError);
    }

    public void close() throws IOException {
        logger.info(String.format("Closing connection to AltUnity Tester on host: %s port: %s.", _host, _port));

        if (this.session != null) {
            this.session.close();
        }
    }
}