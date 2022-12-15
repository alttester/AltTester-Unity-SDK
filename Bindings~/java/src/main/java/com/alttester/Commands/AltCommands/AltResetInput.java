package com.alttester.Commands.AltCommands;

import com.alttester.AltMessage;
import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltResetInput extends AltBaseCommand {
    public AltResetInput(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public String Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("resetInput");
        SendCommand(altMessage);
        return recvall(altMessage, String.class);
    }
}
