package ro.altom.altunitytester.Notifications;

public interface INotificationCallbacks {
    void SceneLoadedCallBack(AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams);
    void SceneUnloadedCallBack(String sceneName);
    void LogCallBack(AltUnityLogNotificationResultParams altUnityLogNotificationResultParams);
    void ApplicationPausedCallBack(boolean applicationPaused);
}