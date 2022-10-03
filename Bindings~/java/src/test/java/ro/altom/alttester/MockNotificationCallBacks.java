package ro.altom.alttester;

import ro.altom.alttester.Logging.AltLogLevel;
import ro.altom.alttester.Notifications.AltLoadSceneNotificationResultParams;
import ro.altom.alttester.Notifications.AltLogNotificationResultParams;
import ro.altom.alttester.Notifications.BaseNotificationCallbacks;

public class MockNotificationCallBacks extends BaseNotificationCallbacks {

    public static String lastLoadedScene;
    public static String lastUnloadedScene;
    public static String logMessage;
    public static String logStackTrace;
    public static AltLogLevel logLevel = AltLogLevel.Error;
    public static boolean applicationPaused;

    @Override
    public void SceneLoadedCallBack(
            AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams) {
        lastLoadedScene = altLoadSceneNotificationResultParams.sceneName;
    }
    
    @Override
    public void SceneUnloadedCallBack(String sceneName) {
        lastUnloadedScene = sceneName;
    }

    @Override
    public void LogCallBack(AltLogNotificationResultParams altLogNotificationResultParams) {
        logMessage = altLogNotificationResultParams.message;
        logStackTrace = altLogNotificationResultParams.stackTrace;
        logLevel = AltLogLevel.values()[altLogNotificationResultParams.level];
    }

    @Override
    public void ApplicationPausedCallBack(boolean applicationPaused) {
        MockNotificationCallBacks.applicationPaused = applicationPaused;
    }
}
