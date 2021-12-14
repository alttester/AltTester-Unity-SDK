package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltUnityAddNotificationListener extends AltBaseCommand {
    private AltUnityAddNotificationListenerParams cmdParams;

    public AltUnityAddNotificationListener(IMessageHandler messageHandler,
            AltUnityAddNotificationListenerParams altUnitySetNotificationParams) {
        super(messageHandler);
        this.cmdParams = altUnitySetNotificationParams;

    }

    public void Execute() {
        messageHandler.addNotificationListener(cmdParams.GetNotificationType(), cmdParams.getNotificationCallbacks(),
                cmdParams.getOverwrite());
        cmdParams.setCommandName("activateNotification");
        SendCommand(cmdParams);
        recvall(cmdParams, String.class);
    }
}
