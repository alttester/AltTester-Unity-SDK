package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltLoadScene extends AltBaseCommand {
    private AltLoadSceneParameters altLoadSceneParameters;

    public AltLoadScene(AltBaseSettings altBaseSettings, AltLoadSceneParameters altLoadSceneParameters) {
        super(altBaseSettings);
        this.altLoadSceneParameters = altLoadSceneParameters;
    }

    public void Execute() {
        SendCommand("loadScene", altLoadSceneParameters.getSceneName(),
                Boolean.toString(altLoadSceneParameters.getLoadSingle()));
        String data = recvall();
        validateResponse("Ok", data);

        data = recvall();
        validateResponse("Scene Loaded", data);
    }
}
