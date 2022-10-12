package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltDriver;
import ro.altom.alttester.Commands.AltBaseCommand;

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
