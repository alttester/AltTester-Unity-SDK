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
    public  void setImplicitTimeout(double value);
    public double getImplicitTimeout();
}
