package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltLoadScene extends AltBaseCommand {
    private AltLoadSceneParams altLoadSceneParameters;

    public AltLoadScene(IMessageHandler messageHandler, AltLoadSceneParams altLoadSceneParameters) {
        super(messageHandler);
        this.altLoadSceneParameters = altLoadSceneParameters;
        this.altLoadSceneParameters.setCommandName("loadScene");
    }

    public void Execute() {
        SendCommand(altLoadSceneParameters);
        String data = recvall(altLoadSceneParameters, String.class);
        validateResponse("Ok", data);

        data = recvall(altLoadSceneParameters, String.class);
        validateResponse("Scene Loaded", data);
    }
}
