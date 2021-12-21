using Altom.AltUnityDriver.Logging;
using Altom.AltUnityDriver.Notifications;

namespace Altom.AltUnityDriver.MockClasses
{
    internal class MockNotificationCallBacks : INotificationCallbacks
    {
        public static string LastSceneLoaded = "";
        public static string LastSceneUnloaded = "";
        public static string LogMessage = "";
        public static AltUnityLogLevel LogLevel = AltUnityLogLevel.Error;
        public static string StackTrace = "";
        public static bool ApplicationPaused = false;
        public void SceneLoadedCallback(AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams)
        {
            LastSceneLoaded = altUnityLoadSceneNotificationResultParams.sceneName;
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            LastSceneUnloaded = sceneName;
        }
        public void LogCallback(AltUnityLogNotificationResultParams altUnityLogNotificationResultParams)
        {
            LogMessage = altUnityLogNotificationResultParams.message;
            LogLevel = altUnityLogNotificationResultParams.level;
            StackTrace = altUnityLogNotificationResultParams.stackTrace;
        }
        public void ApplicationPausedCallback(bool applicationPaused)
        {
            ApplicationPaused = applicationPaused;
        }
    }
}
