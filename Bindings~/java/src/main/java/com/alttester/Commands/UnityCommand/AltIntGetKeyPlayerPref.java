package com.alttester.Commands.UnityCommand;

import com.alttester.IMessageHandler;
import com.alttester.AltDriver;
import com.alttester.Commands.AltBaseCommand;

public class AltIntGetKeyPlayerPref extends AltBaseCommand {

    private AltKeyPlayerPrefParams params;

    public AltIntGetKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
        super(messageHandler);
        params = new AltKeyPlayerPrefParams(keyName, AltDriver.PlayerPrefsKeyType.Int);
        params.setCommandName("getKeyPlayerPref");
    }

    public int Execute() {
        SendCommand(params);
        return recvall(params, Integer.class);
    }
}
