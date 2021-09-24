package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForCurrentSceneToBe extends AltBaseCommand {
    private AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters;

    public AltWaitForCurrentSceneToBe(IMessageHandler messageHandler,
            AltWaitForCurrentSceneToBeParameters altWaitForCurrentSceneToBeParameters) {
        super(messageHandler);
        this.altWaitForCurrentSceneToBeParameters = altWaitForCurrentSceneToBeParameters;
    }

    public String Execute() {
        double time = 0;
        String currentScene = "";
        while (time < altWaitForCurrentSceneToBeParameters.getTimeout()) {
            logger.debug("Waiting for scene to be " + altWaitForCurrentSceneToBeParameters.getSceneName() + "...");
            currentScene = new AltGetCurrentScene(messageHandler).Execute();
            if (currentScene != null && currentScene.equals(altWaitForCurrentSceneToBeParameters.getSceneName())) {
                return currentScene;
            }
            sleepFor(altWaitForCurrentSceneToBeParameters.getInterval());
            time += altWaitForCurrentSceneToBeParameters.getInterval();
        }
        throw new WaitTimeOutException("Scene [" + altWaitForCurrentSceneToBeParameters.getSceneName()
                + "] not loaded after " + altWaitForCurrentSceneToBeParameters.getTimeout() + " seconds");
    }
}
