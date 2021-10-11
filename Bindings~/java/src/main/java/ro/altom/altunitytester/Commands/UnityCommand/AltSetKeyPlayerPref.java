package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSetKeyPlayerPref extends AltBaseCommand {

    private AltSetKeyPlayerPrefParams params;

    public AltSetKeyPlayerPref(IMessageHandler messageHandler, String keyName, int intValue) {
        super(messageHandler);
        params = new AltSetKeyPlayerPrefParams(keyName, intValue);
    }

    public AltSetKeyPlayerPref(IMessageHandler messageHandler, String keyName, float floatValue) {
        super(messageHandler);
        params = new AltSetKeyPlayerPrefParams(keyName, floatValue);
    }

    public AltSetKeyPlayerPref(IMessageHandler messageHandler, String keyName, String stringValue) {
        super(messageHandler);
        params = new AltSetKeyPlayerPrefParams(keyName, stringValue);
    }

    public void Execute() {
        params.setCommandName("setKeyPlayerPref");
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
