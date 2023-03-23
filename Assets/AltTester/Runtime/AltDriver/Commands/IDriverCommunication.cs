using System;
using AltTester.AltTesterUnitySdk.Driver.Notifications;

namespace AltTester.AltTesterUnitySdk.Driver.Commands
{
    public interface IDriverCommunication
    {
        void Send(CommandParams param);
        T Recvall<T>(CommandParams param);
        void AddNotificationListener<T>(NotificationType notificationType, Action<T> callback, bool overwrite);
        void RemoveNotificationListener(NotificationType notificationType);
        void Connect();
        void Close();
        void SetCommandTimeout(int timeout);
        void SetDelayAfterCommand(float delay);
        float GetDelayAfterCommand();
        void SleepFor(float time);
    }
}
