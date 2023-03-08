package com.alttester.Commands;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import com.alttester.AltMessage;
import com.alttester.IMessageHandler;
import com.alttester.Exceptions.AltInvalidServerResponse;

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
            throw new AltInvalidServerResponse(expected, received);
        }
    }
}
