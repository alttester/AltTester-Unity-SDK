using System;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Driver.Notifications;
using AltTester.AltTesterUnitySdk.Communication;
using AltTester.AltTesterUnitySdk.Notification;

namespace AltTester.AltTesterUnitySdk.Commands
{
    public class AltActivateNotificationCommand : AltCommand<ActivateNotification, string>
    {
        ICommandHandler commandHandler;

        public AltActivateNotificationCommand(ICommandHandler commandHandler, ActivateNotification cmdParams) : base(cmdParams)
        {
            this.commandHandler = commandHandler;
        }

        public override string Execute()
        {
            switch (CommandParams.NotificationType)
            {
                case NotificationType.LOADSCENE:
                    new AltLoadSceneNotification(commandHandler, true);
                    break;
                case NotificationType.UNLOADSCENE:
                    new AltUnloadSceneNotification(commandHandler, true);
                    break;
                case NotificationType.LOG:
                    new AltLogNotification(commandHandler, true);
                    break;
                case NotificationType.APPLICATION_PAUSED:
                    new AltTesterApplicationPausedNotification(commandHandler, true);
                    break;
            }
            return "Ok";
        }
    }
}
