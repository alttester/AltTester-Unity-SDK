package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Notifications.INotificationCallbacks;

public class AltUnityAddNotificationListenerParams extends AltMessage {
    private NotificationType notificationType;
    private INotificationCallbacks notificationCallbacks;
    private boolean overwrite;

    public static class Builder {
        NotificationType notificationType;
        INotificationCallbacks notificationCallbacks;
        boolean overwrite = false;

        public Builder(NotificationType notificationType, INotificationCallbacks callbacks) {
            this.notificationType = notificationType;
            this.notificationCallbacks = callbacks;

        }

        public Builder Overwrite(boolean overwrite) {
            this.overwrite = overwrite;
            return this;
        }

        public AltUnityAddNotificationListenerParams build() {
            return new AltUnityAddNotificationListenerParams(notificationType, notificationCallbacks, overwrite);
        }

    }

    AltUnityAddNotificationListenerParams(NotificationType notificationType, INotificationCallbacks callbacks,
            boolean overwrite) {
        this.notificationType = notificationType;
        this.notificationCallbacks = callbacks;
        this.overwrite = overwrite;
    }

    public INotificationCallbacks getNotificationCallbacks() {
        return notificationCallbacks;
    }

    public NotificationType GetNotificationType() {
        return notificationType;
    }

    public boolean getOverwrite() {
        return overwrite;
    }
}
