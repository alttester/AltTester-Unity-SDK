package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltStringGetKeyPlayerPref extends AltBaseCommand {
    private String keyName;
    public AltStringGetKeyPlayerPref(AltBaseSettings altBaseSettings, String keyName) {
        super(altBaseSettings);
        this.keyName = keyName;
    }
    public String Execute(){
        send(CreateCommand("getKeyPlayerPref", keyName, String.valueOf(AltUnityDriver.PlayerPrefsKeyType.StringType)));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}
