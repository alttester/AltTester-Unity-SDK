package ro.altom.altunitytester;

import java.util.LinkedList;
import java.util.Queue;
import java.lang.Thread;

import javax.websocket.Session;
import com.google.gson.Gson;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;

import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;
import ro.altom.altunitytester.altUnityTesterExceptions.CommandResponseTimeoutException;

public class MessageHandler implements IMessageHandler {
    private Session session;
    private Queue<String> responses = new LinkedList<String>();
    private static final Logger logger = LogManager.getLogger(MessageHandler.class);
    private double commandTimeout = 20;

    public MessageHandler(Session session) {
        this.session = session;
    }

    public <T> AltMessageResponse<T> receive(AltMessage altMessage, Class<T> type) {
        double time = 0;
        double delay = 0.1;
        long sleepDelay = (long) (delay * 100);

        while (responses.isEmpty() && session.isOpen() && commandTimeout >= time) {
            try {
                Thread.sleep(sleepDelay);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            time += delay;
        }
        if (commandTimeout < time) {
            throw new CommandResponseTimeoutException();
        }
        if (!session.isOpen()) {
            throw new AltUnityException("Driver disconnected");
        }
        String responseMessage = responses.remove();

        logger.debug("response received: {}", trimLogData(responseMessage));
        AltMessageResponse<T> response = new Gson().fromJson(responseMessage, getType(AltMessageResponse.class, type));
        return response;
    }

    public void send(AltMessage altMessage) {
        String message = new Gson().toJson(altMessage);
        session.getAsyncRemote().sendText(message);
        logger.debug("command sent: {}", trimLogData(message));
    }

    public void onMessage(String message) {
        responses.add(message);
    }

    private String trimLogData(String data) {
        return trimLogData(data, 1024 * 10);
    }

    private String trimLogData(String data, int maxSize) {
        if (data.length() > maxSize) {
            return data.substring(0, 10 * 1024) + "[...]";
        }
        return data;
    }

    private Type getType(Class<?> rawClass, Class<?> parameter) {
        return new ParameterizedType() {
            @Override
            public Type[] getActualTypeArguments() {
                return new Type[] { parameter };
            }

            @Override
            public Type getRawType() {
                return rawClass;
            }

            @Override
            public Type getOwnerType() {
                return null;
            }
        };
    }
}