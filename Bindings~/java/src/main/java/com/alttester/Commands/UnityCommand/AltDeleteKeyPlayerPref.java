package com.alttester.Commands.UnityCommand;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

/**
 * Delete from games player pref a key
 */
public class AltDeleteKeyPlayerPref extends AltBaseCommand {

    private AltKeyPlayerPrefParams params;

    public AltDeleteKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
        super(messageHandler);
        params = new AltKeyPlayerPrefParams(keyName);
        params.setCommandName("deleteKeyPlayerPref");
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
