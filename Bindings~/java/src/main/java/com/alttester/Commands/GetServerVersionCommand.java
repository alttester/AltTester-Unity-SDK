package com.alttester.Commands;

import com.alttester.AltMessage;
import com.alttester.IMessageHandler;

public class GetServerVersionCommand extends AltBaseCommand {
    public GetServerVersionCommand(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public String Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getServerVersion");
        SendCommand(altMessage);
        return recvall(altMessage, String.class);
    }
}
