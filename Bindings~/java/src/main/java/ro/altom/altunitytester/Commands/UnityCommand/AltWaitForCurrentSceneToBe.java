package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.Utils;
import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForCurrentSceneToBe extends AltBaseCommand {
    private AltWaitForCurrentSceneToBeParams params;

    public AltWaitForCurrentSceneToBe(IMessageHandler messageHandler,
            AltWaitForCurrentSceneToBeParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        double time = 0;
        String currentScene = "";
        while (time < params.getTimeout()) {
            logger.debug("Waiting for scene to be " + params.getSceneName() + "...");
            currentScene = new AltGetCurrentScene(messageHandler).Execute();
            if (currentScene != null && currentScene.equals(params.getSceneName())) {
                return;
            }
            Utils.sleepFor(params.getInterval());
            time += params.getInterval();
        }
        throw new WaitTimeOutException("Scene [" + params.getSceneName()
                + "] not loaded after " + params.getTimeout() + " seconds");
    }
}
