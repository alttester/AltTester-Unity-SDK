package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltDriver;
import ro.altom.alttester.Commands.AltBaseCommand;

/**
 * Delete from games player pref a key
 */
public class AltFloatGetKeyPlayerPref extends AltBaseCommand {
    
    private AltKeyPlayerPrefParams params;

    public AltFloatGetKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
        super(messageHandler);
        params = new AltKeyPlayerPrefParams(keyName, AltDriver.PlayerPrefsKeyType.Float);
        params.setCommandName("getKeyPlayerPref");
    }

    public float Execute() {
        SendCommand(params);
        return recvall(params, Float.class);
    }
}
