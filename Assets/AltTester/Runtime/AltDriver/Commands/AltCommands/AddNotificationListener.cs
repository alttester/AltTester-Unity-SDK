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
    public class AddNotificationListener<T> : AltBaseCommand
    {
        private readonly ActivateNotification cmdParams;
        private readonly Action<T> callback;
        private readonly bool overwrite;

        public AddNotificationListener(IDriverCommunication commHandler, NotificationType notificationType, Action<T> callback, bool overwrite) : base(commHandler)
        {
            this.cmdParams = new ActivateNotification(notificationType);
            this.callback = callback;
            this.overwrite = overwrite;
        }
        public void Execute()
        {
            this.CommHandler.AddNotificationListener(cmdParams.NotificationType, callback, overwrite);
            this.CommHandler.Send(this.cmdParams);
            var data = this.CommHandler.Recvall<string>(this.cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}
