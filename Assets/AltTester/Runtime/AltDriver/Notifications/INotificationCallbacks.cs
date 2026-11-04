/*
    Copyright(C) 2026 Altom Consulting
*/


namespace AltTester.AltTesterSDK.Driver.Notifications
{
    public interface INotificationCallbacks
    {
        void SceneLoadedCallback(AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams);
        void SceneUnloadedCallback(string sceneName);
        void LogCallback(AltLogNotificationResultParams altLogNotificationResultParams);
        void ApplicationPausedCallback(bool applicationPaused);
    }
}
