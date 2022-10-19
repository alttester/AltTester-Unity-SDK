package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltKeyUp extends AltBaseCommand {

    private AltKeyUpParams params;

    public AltKeyUp(IMessageHandler messageHandler, AltKeyUpParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        recvall(params, String.class);
    }
}
