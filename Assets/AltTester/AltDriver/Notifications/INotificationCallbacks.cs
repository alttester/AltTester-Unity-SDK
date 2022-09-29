
namespace Altom.AltDriver.Notifications
{
    public interface INotificationCallbacks
    {
        void SceneLoadedCallback(AltLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams);
        void SceneUnloadedCallback(string sceneName);
        void LogCallback(AltLogNotificationResultParams altUnityLogNotificationResultParams);
        void ApplicationPausedCallback(bool applicationPaused);
    }
}