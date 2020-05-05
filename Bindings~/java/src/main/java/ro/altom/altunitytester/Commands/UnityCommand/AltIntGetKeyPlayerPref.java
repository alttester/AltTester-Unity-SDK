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
    public int Execute(){
        send(CreateCommand("getKeyPlayerPref", keyName, String.valueOf(AltUnityDriver.PlayerPrefsKeyType.IntType)));
        String data = recvall();
        if (!data.contains("error:")) {
            return Integer.parseInt(data);
        }
        handleErrors(data);
        return 0;
    }
}
