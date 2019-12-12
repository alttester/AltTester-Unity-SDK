package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltLoadScene extends AltBaseCommand {
    private String sceneName;
    public AltLoadScene(AltBaseSettings altBaseSettings,String sceneName) {
        super(altBaseSettings);
        this.sceneName=sceneName;
    }
    public void Execute(){
//        log.debug("Load scene: " + sceneName + "...");
        send(CreateCommand("loadScene",sceneName ));
        String data = recvall();
        if (data.equals("Ok")) {
            data=recvall();
            if(data.equals("Scene Loaded"))
                return;
        }
        handleErrors(data);
    }
}
