using System;
using Altom.AltUnityDriver.Logging;
using NLog;
namespace Altom.AltUnityDriver.Notifications
{
    public class BaseNotificationCallBacks : INotificationCallbacks
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        public void SceneLoadedCallback(AltUnityLoadSceneNotificationResultParams altUnityLoadSceneNotificationResultParams)
        {
            logger.Log(LogLevel.Info, String.Format("Scene {0} was loaded {1}", altUnityLoadSceneNotificationResultParams.sceneName, altUnityLoadSceneNotificationResultParams.loadSceneMode.ToString()));
        }
        public void HierarchyChangesCallback(AltUnityHierarchyNotificationResultParams altUnityHierarchyNotificationResultParams)
        {
            logger.Log(LogLevel.Info, String.Format("The objects in hierarchy were {0}", altUnityHierarchyNotificationResultParams.hierarchyMode.ToString()));
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            logger.Log(LogLevel.Info, String.Format("Scene {0} was unloaded", sceneName));
        }
    }
}