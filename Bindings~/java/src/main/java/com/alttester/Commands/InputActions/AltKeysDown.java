package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltKeysDown extends AltBaseCommand {

    private AltKeysDownParams altKeysDownParameters;

    public AltKeysDown(IMessageHandler messageHandler, AltKeysDownParams altKeysDownParameters) {
        super(messageHandler);
        this.altKeysDownParameters = altKeysDownParameters;
    }

    public void Execute() {
        SendCommand(altKeysDownParameters);
        recvall(altKeysDownParameters, String.class);
    }
}
