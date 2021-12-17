using System;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver.Notifications;
using Altom.AltUnityTester.Communication;
using Altom.AltUnityTester.Notification;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityActivateNotificationCommand : AltUnityCommand<ActivateNotification, string>
    {
        ICommandHandler commandHandler;
        public AltUnityActivateNotificationCommand(ICommandHandler commandHandler, ActivateNotification cmdParams) : base(cmdParams)
        {
            this.commandHandler = commandHandler;

        }
        public override string Execute()
        {
            switch (CommandParams.NotificationType)
            {
                case NotificationType.LOADSCENE:
                    new AltUnityLoadSceneNotification(commandHandler, true);
                    break;
                case NotificationType.UNLOADSCENE:
                    new AltUnityUnloadSceneNotification(commandHandler, true);
                    break;
                case NotificationType.HIERARCHYCHANGED:
                    new AltUnityHierarchyChangedNotification(commandHandler, true);
                    break;
            }
            return "Ok";
        }

    }
}