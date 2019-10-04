package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltDeleteKeyPlayerPref extends AltBaseCommand {
    private String keyName;
    public AltDeleteKeyPlayerPref(AltBaseSettings altBaseSettings, String keyName) {
        super(altBaseSettings);
        this.keyName = keyName;
    }
    public void Execute(){
        send(CreateCommand("deleteKeyPlayerPref", keyName));
        String data = recvall();
        if (data.equals("Ok"))
            return;
        handleErrors(data);
    }
}
