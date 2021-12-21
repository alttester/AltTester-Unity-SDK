using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver.Notifications
{
    public class AltUnityLogNotificationResultParams
    {
        public string message;
        public string stackTrace;
        public AltUnityLogLevel level;

        public AltUnityLogNotificationResultParams(string message, string stackTrace, AltUnityLogLevel level)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.level = level;
        }
    }
}