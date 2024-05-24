/*
    Copyright(C) 2024 Altom Consulting

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

using System;
using AltTester.AltTesterUnitySDK.Driver.Notifications;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public interface IDriverCommunication
    {
        void Send(CommandParams param);
        T Recvall<T>(CommandParams param);
        void AddNotificationListener<T>(NotificationType notificationType, Action<T> callback, bool overwrite);
        void RemoveNotificationListener(NotificationType notificationType);
        void Connect();
        void Close();
        void SetCommandTimeout(int timeout);
        void SetDelayAfterCommand(float delay);
        float GetDelayAfterCommand();
        void SleepFor(float time);
    }
}
