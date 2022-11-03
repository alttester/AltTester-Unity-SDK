package com.alttester.Commands.AltCommands;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

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
