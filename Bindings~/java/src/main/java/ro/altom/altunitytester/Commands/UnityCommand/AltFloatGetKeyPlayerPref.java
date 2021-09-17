package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Delete from games player pref a key
 */
public class AltFloatGetKeyPlayerPref extends AltBaseCommand {
    
    private AltKeyPlayerPrefParams params;

    public AltFloatGetKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
        super(messageHandler);
        params = new AltKeyPlayerPrefParams(keyName, AltUnityDriver.PlayerPrefsKeyType.Float);
        params.setCommandName("getKeyPlayerPref");
    }

    public float Execute() {
        SendCommand(params);
        return recvall(params, Float.class);
    }
}
