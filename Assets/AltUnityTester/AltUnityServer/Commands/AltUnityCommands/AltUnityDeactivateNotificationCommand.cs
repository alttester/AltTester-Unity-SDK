using System;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver.Notifications;
using Altom.AltUnityTester.Communication;
using Altom.AltUnityTester.Notification;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityDeactivateNotificationCommand : AltUnityCommand<DeactivateNotification, string>
    {
        ICommandHandler commandHandler;
        public AltUnityDeactivateNotificationCommand(ICommandHandler commandHandler, DeactivateNotification cmdParams) : base(cmdParams)
        {
            this.commandHandler = commandHandler;

        }
        public override string Execute()
        {
            switch (CommandParams.NotificationType)
            {
                case NotificationType.LOADSCENE:
                    new AltUnityLoadSceneNotification(commandHandler, false);
                    break;
                case NotificationType.UNLOADSCENE:
                    new AltUnityUnloadSceneNotification(commandHandler, false);
                    break;
                case NotificationType.LOG:
                    new AltUnityLogNotification(commandHandler, true);
                    break;
                case NotificationType.APPLICATION_PAUSED:
                    new AltUnityApplicationPausedNotification(commandHandler, false);
                    break;
            }
            return "Ok";
        }

    }
}