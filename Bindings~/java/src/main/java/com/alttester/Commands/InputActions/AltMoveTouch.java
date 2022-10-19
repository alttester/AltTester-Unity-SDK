package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltMoveTouch extends AltBaseCommand {
    private AltMoveTouchParams params;

    public AltMoveTouch(IMessageHandler messageHandler, AltMoveTouchParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
