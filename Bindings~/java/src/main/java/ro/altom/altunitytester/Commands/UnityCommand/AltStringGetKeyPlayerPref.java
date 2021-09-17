package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltStringGetKeyPlayerPref extends AltBaseCommand {
    
    private AltKeyPlayerPrefParams params;

    public AltStringGetKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
        super(messageHandler);
        params = new AltKeyPlayerPrefParams(keyName, AltUnityDriver.PlayerPrefsKeyType.String);
        params.setCommandName("getKeyPlayerPref");
    }

    public String Execute() {
        SendCommand(params);
        return recvall(params, String.class);
    }
}
