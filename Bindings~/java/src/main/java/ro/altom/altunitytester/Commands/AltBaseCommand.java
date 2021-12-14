package ro.altom.altunitytester.Commands;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

public class AltBaseCommand {
    protected static final Logger logger = LogManager.getLogger(AltBaseCommand.class);

    protected IMessageHandler messageHandler;

    public AltBaseCommand(IMessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    protected <T> T recvall(AltMessage altMessage, final Class<T> type) {

        return messageHandler.receive(altMessage, type);
    }

    protected void SendCommand(AltMessage altMessage) {
        altMessage.setMessageId(Long.toString(System.currentTimeMillis()));
        messageHandler.send(altMessage);
    }

    protected void validateResponse(String expected, String received) {
        if (!expected.equals(received)) {
            throw new AltUnityInvalidServerResponse(expected, received);
        }
    }

    /**
     * Sleeps for certain amount of seconds.
     *
     * @param interval Seconds to sleep for.
     */
    protected void sleepFor(double interval) {
        long timeToSleep = (long) (interval * 1000);
        try {
            Thread.sleep(timeToSleep);
        } catch (InterruptedException e) {
            logger.warn("Could not sleep for " + timeToSleep + " ms");
        }
    }
}
