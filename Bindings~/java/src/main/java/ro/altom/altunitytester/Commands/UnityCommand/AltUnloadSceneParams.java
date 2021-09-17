package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;

public class AltUnloadSceneParams extends AltMessage{
    
    private String sceneName;

    AltUnloadSceneParams(String sceneName){
        this.setSceneName(sceneName);
    }

    public String getSceneName() {
        return sceneName;
    }

    public void setSceneName(String sceneName) {
        this.sceneName = sceneName;
    }
}
