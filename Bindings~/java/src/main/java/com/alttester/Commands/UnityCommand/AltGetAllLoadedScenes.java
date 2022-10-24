package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltGetAllLoadedScenes extends AltBaseCommand {
    public AltGetAllLoadedScenes(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public String[] Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getAllLoadedScenes");
        SendCommand(altMessage);
        return recvall(altMessage, String[].class);
    }
}
