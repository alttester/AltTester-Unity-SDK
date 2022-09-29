using System;
using Altom.AltDriver.Logging;

namespace Altom.AltDriver.Notifications
{
    public class BaseNotificationCallBacks : INotificationCallbacks
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        public void SceneLoadedCallback(AltLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Scene {0} was loaded {1}", altUnityLoadSceneNotificationResultParams.sceneName, altUnityLoadSceneNotificationResultParams.loadSceneMode.ToString()));
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Scene {0} was unloaded", sceneName));
        }
        public void LogCallback(AltLogNotificationResultParams altUnityLogNotificationResultParams)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Log of type {0} with message {1} was received", altUnityLogNotificationResultParams.level, altUnityLogNotificationResultParams.message));
        }
        public void ApplicationPausedCallback(bool applicationPaused)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Application paused: {0}", applicationPaused));
        }
    }
}