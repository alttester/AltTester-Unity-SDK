package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltDriver;
import ro.altom.alttester.Commands.AltBaseCommand;

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
