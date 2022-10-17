package ro.altom.alttester.Commands.AltCommands;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltRemoveNotificationListener extends AltBaseCommand {
    private AltRemoveNotificationListenerParams cmdParams;

    public AltRemoveNotificationListener(IMessageHandler messageHandler,
            AltRemoveNotificationListenerParams altSetNotificationParams) {
        super(messageHandler);
        this.cmdParams = altSetNotificationParams;
    }

    public void Execute() {
        messageHandler.removeNotificationListener(cmdParams.GetNotificationType());
        cmdParams.setCommandName("deactivateNotification");
        SendCommand(cmdParams);
        recvall(cmdParams, String.class);
    }
}
