package ro.altom.altunitytester.Notifications;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

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

}