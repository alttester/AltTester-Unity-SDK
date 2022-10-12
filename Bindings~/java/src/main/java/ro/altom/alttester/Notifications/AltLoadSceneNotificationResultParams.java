package ro.altom.alttester.Notifications;

public class AltLoadSceneNotificationResultParams {
    public String sceneName;
    public LoadSceneMode loadSceneMode;

    public AltLoadSceneNotificationResultParams(String sceneName, LoadSceneMode loadSceneMode) {
        this.sceneName = sceneName;
        this.loadSceneMode = loadSceneMode;
    }
}
