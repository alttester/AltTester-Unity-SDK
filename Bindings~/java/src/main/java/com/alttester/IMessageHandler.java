package com.alttester;

import com.alttester.Commands.AltCommands.NotificationType;
import com.alttester.Notifications.INotificationCallbacks;

public interface IMessageHandler {

    public <T> T receive(AltMessage altMessage, Class<T> type);

    public void send(AltMessage altMessage);

    public void onMessage(String message);

    public void setCommandTimeout(int timeout);

    public void addNotificationListener(NotificationType notificationType, INotificationCallbacks callbacks,
            boolean overwrite);

    public void removeNotificationListener(NotificationType notificationType);

    public double getDelayAfterCommand();

    public void setDelayAfterCommand(double delay);
}
