package ro.altom.altunitytester.Notifications;

public interface INotificationCallbacks {
    void SceneLoadedCallBack(AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams);
    void SceneUnloadedCallBack(String sceneName);
    void ApplicationPausedCallBack(boolean applicationPaused);
}