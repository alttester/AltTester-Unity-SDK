using System.Reflection;
using Altom.AltUnityDriver.Notifications;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Notification
{
    public class AltUnityApplicationPausedNotification : BaseNotification
    {
        static bool send = false;
        public AltUnityApplicationPausedNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            send = isOn;
        }

        public static void OnPause(bool pauseStatus)
        {
            if (send)
                SendNotification(pauseStatus, "applicationPausedNotification");
        }

    }
}