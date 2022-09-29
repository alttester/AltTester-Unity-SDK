using Altom.AltDriver.Logging;
using Altom.AltDriver.Notifications;

namespace Altom.AltDriver.MockClasses
{
    internal class MockNotificationCallBacks : INotificationCallbacks
    {
        public static string LastSceneLoaded = "";
        public static string LastSceneUnloaded = "";
        public static string LogMessage = "";
        public static AltLogLevel LogLevel = AltLogLevel.Error;
        public static string StackTrace = "";
        public static bool ApplicationPaused = false;
        public void SceneLoadedCallback(AltLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams)
        {
            LastSceneLoaded = altUnityLoadSceneNotificationResultParams.sceneName;
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            LastSceneUnloaded = sceneName;
        }
        public void LogCallback(AltLogNotificationResultParams altUnityLogNotificationResultParams)
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
