package com.alttester.Commands.AltCommands;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

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
