package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.AltMessage;

public class AltUnityRemoveNotificationListenerParams extends AltMessage {
    private NotificationType notificationType;

    public static class Builder {
        NotificationType notificationType;

        public Builder(NotificationType notificationType) {
            this.notificationType = notificationType;

        }

        public AltUnityRemoveNotificationListenerParams build() {
            return new AltUnityRemoveNotificationListenerParams(notificationType);
        }

    }

    AltUnityRemoveNotificationListenerParams(NotificationType notificationType) {
        this.notificationType = notificationType;
    }

    public NotificationType GetNotificationType() {
        return notificationType;
    }

}