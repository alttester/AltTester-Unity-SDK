package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltLoadScene extends AltBaseCommand {
    private AltLoadSceneParameters altLoadSceneParameters;
    public AltLoadScene(AltBaseSettings altBaseSettings,AltLoadSceneParameters altLoadSceneParameters) {
        super(altBaseSettings);
        this.altLoadSceneParameters=altLoadSceneParameters;
    }
    public void Execute(){
        send(CreateCommand("loadScene",altLoadSceneParameters.getSceneName(),Boolean.toString(altLoadSceneParameters.getLoadSingle() )));
        String data = recvall();
        if (data.equals("Ok")) {
            data=recvall();
            if(data.equals("Scene Loaded"))
                return;
        }
        handleErrors(data);
    }
}
