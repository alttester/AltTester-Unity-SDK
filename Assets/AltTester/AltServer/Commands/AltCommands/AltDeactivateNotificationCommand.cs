using System;
using AltTester.AltDriver.Commands;
using AltTester.AltDriver.Notifications;
using AltTester.Communication;
using AltTester.Notification;

namespace AltTester.Commands
{
    public class AltDeactivateNotificationCommand : AltCommand<DeactivateNotification, string>
    {
        ICommandHandler commandHandler;
        public AltDeactivateNotificationCommand(ICommandHandler commandHandler, DeactivateNotification cmdParams) : base(cmdParams)
        {
            this.commandHandler = commandHandler;

        }
        public override string Execute()
        {
            switch (CommandParams.NotificationType)
            {
                case NotificationType.LOADSCENE:
                    new AltLoadSceneNotification(commandHandler, false);
                    break;
                case NotificationType.UNLOADSCENE:
                    new AltUnloadSceneNotification(commandHandler, false);
                    break;
                case NotificationType.LOG:
                    new AltLogNotification(commandHandler, true);
                    break;
                case NotificationType.APPLICATION_PAUSED:
                    new AltTesterApplicationPausedNotification(commandHandler, false);
                    break;
            }
            return "Ok";
        }

    }
}