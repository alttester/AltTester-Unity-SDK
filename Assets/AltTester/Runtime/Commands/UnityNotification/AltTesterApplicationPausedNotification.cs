using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Notification
{
    public class AltTesterApplicationPausedNotification : BaseNotification
    {
        static bool send = false;
        public AltTesterApplicationPausedNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
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