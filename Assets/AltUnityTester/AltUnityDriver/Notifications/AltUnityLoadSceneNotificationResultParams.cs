
namespace Altom.AltUnityDriver.Notifications
{
    public class AltUnityLoadSceneNotificationResultParams
    {
        public string sceneName;
        public AltUnityLoadSceneMode loadSceneMode;

        public AltUnityLoadSceneNotificationResultParams(string sceneName, AltUnityLoadSceneMode loadSceneMode)
        {
            this.sceneName = sceneName;
            this.loadSceneMode = loadSceneMode;
        }
    }
}