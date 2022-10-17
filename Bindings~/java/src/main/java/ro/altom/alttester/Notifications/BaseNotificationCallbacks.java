package ro.altom.alttester.Notifications;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import ro.altom.alttester.Logging.AltLogLevel;

public class BaseNotificationCallbacks implements INotificationCallbacks {
    protected static final Logger logger = LogManager.getLogger(BaseNotificationCallbacks.class);

    @Override
    public void SceneLoadedCallBack(
            AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams) {
        logger.info("Scene " + altLoadSceneNotificationResultParams.sceneName + " was loaded "
                + altLoadSceneNotificationResultParams.loadSceneMode);
    }

    @Override
    public void SceneUnloadedCallBack(String sceneName) {
        logger.info("Scene " + sceneName + " was unloaded ");
    }
    
    @Override
    public void LogCallBack(AltLogNotificationResultParams altLogNotificationResultParams) {
        logger.info("Log of type " + AltLogLevel.values()[altLogNotificationResultParams.level] + " with message " +
        altLogNotificationResultParams.message + " was received");
    }

    @Override
    public void ApplicationPausedCallBack(boolean applicationPaused) {
        logger.info("Application paused: " + applicationPaused);
    }
}