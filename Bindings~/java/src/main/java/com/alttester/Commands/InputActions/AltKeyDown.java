package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltKeyDown extends AltBaseCommand {

    private AltKeyDownParams altKeyDownParameters;

    public AltKeyDown(IMessageHandler messageHandler, AltKeyDownParams altKeyDownParameters) {
        super(messageHandler);
        this.altKeyDownParameters = altKeyDownParameters;
    }

    public void Execute() throws InterruptedException {
        SendCommand(altKeyDownParameters);
        recvall(altKeyDownParameters, String.class);
        Thread.sleep(100);
    }
}
