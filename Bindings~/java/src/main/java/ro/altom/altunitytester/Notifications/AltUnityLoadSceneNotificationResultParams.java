package ro.altom.altunitytester.Notifications;

public class AltUnityLoadSceneNotificationResultParams {
    public String sceneName;
    public LoadSceneMode loadSceneMode;

    public AltUnityLoadSceneNotificationResultParams(String sceneName, LoadSceneMode loadSceneMode) {
        this.sceneName = sceneName;
        this.loadSceneMode = loadSceneMode;
    }
}
