/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester.Commands.AltCommands;

import com.alttester.AltMessage;
import com.alttester.Notifications.INotificationCallbacks;

public class AltAddNotificationListenerParams extends AltMessage {
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

        public AltAddNotificationListenerParams build() {
            return new AltAddNotificationListenerParams(notificationType, notificationCallbacks, overwrite);
        }

    }

    AltAddNotificationListenerParams(NotificationType notificationType, INotificationCallbacks callbacks,
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
