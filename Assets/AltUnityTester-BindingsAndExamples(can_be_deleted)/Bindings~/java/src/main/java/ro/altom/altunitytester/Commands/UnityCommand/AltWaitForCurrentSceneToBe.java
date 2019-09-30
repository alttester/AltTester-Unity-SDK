package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForCurrentSceneToBe extends AltBaseCommand {
    private AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters;
    public AltWaitForCurrentSceneToBe(AltBaseSettings altBaseSettings, AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters) {
        super(altBaseSettings);
        this.altWaitForCurrentSceneToBeParameters = altWaitForCurrentSceneToBeParameters;
    }
    public String Execute(){
        double time = 0;
        String currentScene = "";
        while (time < altWaitForCurrentSceneToBeParameters.getTimeout()) {
            log.debug("Waiting for scene to be " + altWaitForCurrentSceneToBeParameters.getSceneName()+ "...");
            currentScene = new AltGetCurrentScene(altBaseSettings).Execute();
            if (currentScene != null && currentScene.equals(altWaitForCurrentSceneToBeParameters.getSceneName())) {
                return currentScene;
            }
            sleepFor(altWaitForCurrentSceneToBeParameters.getInterval());
            time += altWaitForCurrentSceneToBeParameters.getInterval();
        }
        throw new WaitTimeOutException("Scene [" + altWaitForCurrentSceneToBeParameters.getSceneName()+ "] not loaded after " + altWaitForCurrentSceneToBeParameters.getTimeout()+ " seconds");
    }
}
