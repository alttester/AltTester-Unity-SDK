package ro.altom.altunitytester;

import java.io.IOException;

import javax.websocket.ClientEndpoint;
import javax.websocket.ContainerProvider;
import javax.websocket.DeploymentException;
import javax.websocket.Session;
import javax.websocket.WebSocketContainer;
import java.net.URI;
import java.util.LinkedList;
import java.util.Queue;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import javax.websocket.EndpointConfig;
import javax.websocket.OnClose;
import javax.websocket.OnError;
import javax.websocket.OnMessage;
import javax.websocket.OnOpen;

import ro.altom.altunitytester.altUnityTesterExceptions.*;

@ClientEndpoint
public class WebsocketConnection {
    private static final Logger logger = LogManager.getLogger(AltUnityDriver.class);
    public Session session = null;
    public IMessageHandler messageHandler = null;

    @OnOpen
    public void onOpen(Session session) {
        logger.info("webscoket connection open");
        this.session = session;
        this.messageHandler = new MessageHandler(session);

        System.out.println("[Session establishment]");
    }

    @OnMessage
    public void onMessage(String message) {
        System.out.println("[Receive]:" + message);
        messageHandler.onMessage(message);

    }

    // Processing when receiving a message
    @OnError
    public void onError(Throwable th) {
        System.err.println(th.getMessage());
    }

    // Processing at session release
    @OnClose
    public void onClose(Session session) {
        System.out.println("[Disconnect]");
    }

    public WebsocketConnection(String host, int port) {
        final String uri = "ws://" + host + ":" + port + "/altws/";

        try {
            WebSocketContainer container = ContainerProvider.getWebSocketContainer();
            this.session = container.connectToServer(this, URI.create(uri));
        } catch (DeploymentException | IllegalStateException e) {
            logger.error(e);
            throw new ConnectionException(e.getMessage(), e);
        } catch (IOException e) {
            logger.error(e);
            throw new ConnectionException("Could not create connection to " + uri, e);
        }
    }

}