package ro.altom.altunitytester;

import ro.altom.altunitytester.Notifications.AltUnityLoadSceneNotificationResultParams;
import ro.altom.altunitytester.Notifications.BaseNotificationCallbacks;

public class MockNotificationCallBacks extends BaseNotificationCallbacks {

    public static String lastLoadedScene;
    public static String lastUnloadedScene;

    @Override
    public void SceneLoadedCallBack(
            AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams) {
        lastLoadedScene = altUnityLoadSceneNotificationResultParams.sceneName;
    }

    @Override
    public void SceneUnloadedCallBack(String sceneName) {
        lastUnloadedScene = sceneName;
    }

}
