package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltKeysUp extends AltBaseCommand {

    private AltKeysUpParams params;

    public AltKeysUp(IMessageHandler messageHandler, AltKeysUpParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        recvall(params, String.class);
    }
}
