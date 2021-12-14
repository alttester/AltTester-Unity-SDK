package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltUnityRemoveNotificationListener extends AltBaseCommand {
    private AltUnityRemoveNotificationListenerParams cmdParams;

    public AltUnityRemoveNotificationListener(IMessageHandler messageHandler,
            AltUnityRemoveNotificationListenerParams altUnitySetNotificationParams) {
        super(messageHandler);
        this.cmdParams = altUnitySetNotificationParams;
    }

    public void Execute() {
        messageHandler.removeNotificationListener(cmdParams.GetNotificationType());
        cmdParams.setCommandName("deactivateNotification");
        SendCommand(cmdParams);
        recvall(cmdParams, String.class);
    }
}
