package com.alttester.Commands.UnityCommand;

import com.alttester.IMessageHandler;
import com.alttester.AltDriver;
import com.alttester.Commands.AltBaseCommand;

public class AltStringGetKeyPlayerPref extends AltBaseCommand {

    private AltKeyPlayerPrefParams params;

    public AltStringGetKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
        super(messageHandler);
        params = new AltKeyPlayerPrefParams(keyName, AltDriver.PlayerPrefsKeyType.String);
        params.setCommandName("getKeyPlayerPref");
    }

    public String Execute() {
        SendCommand(params);
        return recvall(params, String.class);
    }
}
