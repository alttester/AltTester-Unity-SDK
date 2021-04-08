package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltIntGetKeyPlayerPref extends AltBaseCommand {
    private String keyName;

    public AltIntGetKeyPlayerPref(AltBaseSettings altBaseSettings, String keyName) {
        super(altBaseSettings);
        this.keyName = keyName;
    }

    public int Execute() {
        SendCommand("getKeyPlayerPref", keyName, String.valueOf(AltUnityDriver.PlayerPrefsKeyType.IntType));
        String data = recvall();
        return Integer.parseInt(data);
    }
}
