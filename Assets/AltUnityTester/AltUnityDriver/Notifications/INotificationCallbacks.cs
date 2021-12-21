
namespace Altom.AltUnityDriver.Notifications
{
    public interface INotificationCallbacks
    {
        void SceneLoadedCallback(AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams);
        void SceneUnloadedCallback(string sceneName);
        void LogCallback(AltUnityLogNotificationResultParams altUnityLogNotificationResultParams);
        void ApplicationPausedCallback(bool applicationPaused);
    }
}