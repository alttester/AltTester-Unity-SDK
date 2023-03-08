package com.alttester;

import com.alttester.Commands.AltCommands.NotificationType;
import com.alttester.Notifications.INotificationCallbacks;

public interface IMessageHandler {
    <T> T receive(AltMessage altMessage, Class<T> type);

    void send(AltMessage altMessage);

    void onMessage(String message);

    void setCommandTimeout(int timeout);

    void addNotificationListener(NotificationType notificationType, INotificationCallbacks callbacks,
            boolean overwrite);

    void removeNotificationListener(NotificationType notificationType);

    double getDelayAfterCommand();

    void setDelayAfterCommand(double delay);
}
