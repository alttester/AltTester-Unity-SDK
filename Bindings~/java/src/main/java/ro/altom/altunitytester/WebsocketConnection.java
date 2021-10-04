package ro.altom.altunitytester;

import java.io.IOException;

import javax.websocket.ClientEndpoint;
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
    }

    // Processing at session release
    @OnClose
    public void onClose(Session session) {
    }

    public WebsocketConnection(String host, int port, int connectTimeout) {
        _uri = "ws://" + host + ":" + port + "/altws/";

        logger.debug("Connecting to: {}", _uri);
        long start = System.currentTimeMillis();
        long finish = System.currentTimeMillis();
        int retries = 0;

        WebSocketContainer container = ContainerProvider.getWebSocketContainer();
        boolean connected = false;
        Exception connectionError = null;
        while (!connected && finish - start < connectTimeout * 1000) {
            try {
                if (retries > 0)
                    logger.debug("Retrying #{} to {}", retries, _uri);
                this.session = container.connectToServer(this, URI.create(_uri));
                connected = true;
            } catch (IllegalStateException e) {
                logger.error(e);
                throw new ConnectionException(e.getMessage(), e);
            } catch (DeploymentException | IOException e) {
                connectionError = e;
            }
            retries++;
            finish = System.currentTimeMillis();
        }
        if (!connected) {
            logger.error(connectionError);
            throw new ConnectionException("Could not create connection to " + _uri, connectionError);
        }
    }
}