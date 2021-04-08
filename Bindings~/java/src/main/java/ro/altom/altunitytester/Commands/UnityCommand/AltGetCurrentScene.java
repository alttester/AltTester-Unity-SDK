package ro.altom.altunitytester.Commands.UnityCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltGetCurrentScene extends AltBaseCommand {
    public AltGetCurrentScene(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }

    public String Execute() {
        SendCommand("getCurrentScene");
        String data = recvall();
        return (new Gson().fromJson(data, AltUnityObject.class)).name;
    }
}
