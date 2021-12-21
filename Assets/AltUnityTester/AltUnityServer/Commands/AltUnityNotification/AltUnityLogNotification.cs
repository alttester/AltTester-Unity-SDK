using System.Reflection;
using Altom.AltUnityDriver.Notifications;
using Altom.AltUnityTester.Communication;
using UnityEngine;
using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityTester.Notification
{
    public class AltUnityLogNotification : BaseNotification
    {
        public AltUnityLogNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            Application.logMessageReceived -= onLogReceived;

            if (isOn)
            {
                Application.logMessageReceived += onLogReceived;
            }

        }

        static void onLogReceived(string message, string stackTrace, LogType type)
        {
            var data = new AltUnityLogNotificationResultParams(message, stackTrace, logTypeToLogLevel(type));
            SendNotification(data, "logNotification");
        }

        private static AltUnityLogLevel logTypeToLogLevel(LogType type)
        {
            if((int)type == (int)LogType.Error || (int)type == (int)LogType.Exception)
                return AltUnityLogLevel.Error;
            else if ((int)type == (int)LogType.Assert || (int)type == (int)LogType.Log)
                return AltUnityLogLevel.Debug;
            return AltUnityLogLevel.Warn;
        }
    }
}