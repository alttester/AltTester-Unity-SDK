package ro.altom.alttester.Commands.AltCommands;

import ro.altom.alttester.AltMessage;

public class AltRemoveNotificationListenerParams extends AltMessage {
    private NotificationType notificationType;

    public static class Builder {
        NotificationType notificationType;

        public Builder(NotificationType notificationType) {
            this.notificationType = notificationType;

        }

        public AltRemoveNotificationListenerParams build() {
            return new AltRemoveNotificationListenerParams(notificationType);
        }

    }

    AltRemoveNotificationListenerParams(NotificationType notificationType) {
        this.notificationType = notificationType;
    }

    public NotificationType GetNotificationType() {
        return notificationType;
    }

}