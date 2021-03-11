package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltUnloadScene extends AltBaseCommand {
    private String sceneName;

    public AltUnloadScene(AltBaseSettings altBaseSettings, String sceneName) {
        super(altBaseSettings);
        this.sceneName = sceneName;
    }

    public void Execute() {
        SendCommand("unloadScene", sceneName);
        String data = recvall();
        if (data.equals("Ok")) {
            data = recvall();
            if (data.equals("Scene Unloaded"))
                return;
        }
        handleErrors(data);
    }
}
