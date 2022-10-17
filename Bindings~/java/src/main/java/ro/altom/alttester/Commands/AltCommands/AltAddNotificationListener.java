package ro.altom.alttester.Commands.AltCommands;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltAddNotificationListener extends AltBaseCommand {
    private AltAddNotificationListenerParams cmdParams;

    public AltAddNotificationListener(IMessageHandler messageHandler,
            AltAddNotificationListenerParams altSetNotificationParams) {
        super(messageHandler);
        this.cmdParams = altSetNotificationParams;

    }

    public void Execute() {
        messageHandler.addNotificationListener(cmdParams.GetNotificationType(), cmdParams.getNotificationCallbacks(),
                cmdParams.getOverwrite());
        cmdParams.setCommandName("activateNotification");
        SendCommand(cmdParams);
        recvall(cmdParams, String.class);
    }
}
