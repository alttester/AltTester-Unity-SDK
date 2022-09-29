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
        public void SceneLoadedCallback(AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams)
        {
            LastSceneLoaded = altLoadSceneNotificationResultParams.sceneName;
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            LastSceneUnloaded = sceneName;
        }
        public void LogCallback(AltLogNotificationResultParams altLogNotificationResultParams)
        {
            LogMessage = altLogNotificationResultParams.message;
            LogLevel = altLogNotificationResultParams.level;
            StackTrace = altLogNotificationResultParams.stackTrace;
        }
        public void ApplicationPausedCallback(bool applicationPaused)
        {
            ApplicationPaused = applicationPaused;
        }
    }
}
