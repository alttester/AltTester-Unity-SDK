package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltBeginTouch extends AltBaseCommand {
    private AltBeginTouchParams params;

    public AltBeginTouch(IMessageHandler messageHandler, AltBeginTouchParams params) {
        super(messageHandler);
        this.params = params;
    }

    public int Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        return Integer.parseInt(data);
    }
}
