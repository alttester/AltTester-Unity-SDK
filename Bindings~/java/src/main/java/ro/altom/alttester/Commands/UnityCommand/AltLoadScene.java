package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

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
