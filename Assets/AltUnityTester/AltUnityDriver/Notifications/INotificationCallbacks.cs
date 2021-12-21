
namespace Altom.AltUnityDriver.Notifications
{
    public interface INotificationCallbacks
    {
        void SceneLoadedCallback(AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams);
        void SceneUnloadedCallback(string sceneName);
        void ApplicationPausedCallback(bool applicationPaused);
    }
}