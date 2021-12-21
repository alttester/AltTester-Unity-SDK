package ro.altom.altunitytester.Notifications;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import ro.altom.altunitytester.Logging.AltUnityLogLevel;

public class BaseNotificationCallbacks implements INotificationCallbacks {
    protected static final Logger logger = LogManager.getLogger(BaseNotificationCallbacks.class);

    @Override
    public void SceneLoadedCallBack(
            AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams) {
        logger.info("Scene " + altUnityLoadSceneNotificationResultParams.sceneName + " was loaded "
                + altUnityLoadSceneNotificationResultParams.loadSceneMode);
    }

    @Override
    public void SceneUnloadedCallBack(String sceneName) {
        logger.info("Scene " + sceneName + " was unloaded ");
    }
    
    @Override
    public void LogCallBack(AltUnityLogNotificationResultParams altUnityLogNotificationResultParams) {
        logger.info("Log of type " + AltUnityLogLevel.values()[altUnityLogNotificationResultParams.level] + " with message " +
        altUnityLogNotificationResultParams.message + " was received");
    }

    @Override
    public void ApplicationPausedCallBack(boolean applicationPaused) {
        logger.info("Application paused: " + applicationPaused);
    }
}