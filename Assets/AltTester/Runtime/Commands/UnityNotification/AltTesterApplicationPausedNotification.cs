/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterUnitySDK.Commands;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class AltTesterApplicationPausedNotification : BaseNotification
    {
        static bool send = false;
        static bool isPaused = false;

        public AltTesterApplicationPausedNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            bool wasSending = send;
            send = isOn;
            if (isOn && !wasSending && isPaused)
                SendNotification(isPaused, "applicationPausedNotification");
        }

        public static void OnPause(bool pauseStatus)
        {
            isPaused = pauseStatus;
            if (send)
                SendNotification(pauseStatus, "applicationPausedNotification");
        }
        public static void StopApplicationPaused()
        {
            send = false;
        }

    }
}
