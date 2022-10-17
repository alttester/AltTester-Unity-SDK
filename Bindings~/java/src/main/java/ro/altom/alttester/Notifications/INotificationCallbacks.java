package ro.altom.alttester.Notifications;

public interface INotificationCallbacks {
    void SceneLoadedCallBack(AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams);
    void SceneUnloadedCallBack(String sceneName);
    void LogCallBack(AltLogNotificationResultParams altLogNotificationResultParams);
    void ApplicationPausedCallBack(boolean applicationPaused);
}