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
