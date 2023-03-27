package com.alttester;

import com.alttester.Logging.AltLogLevel;
import com.alttester.Notifications.AltLoadSceneNotificationResultParams;
import com.alttester.Notifications.AltLogNotificationResultParams;
import com.alttester.Notifications.BaseNotificationCallbacks;

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
