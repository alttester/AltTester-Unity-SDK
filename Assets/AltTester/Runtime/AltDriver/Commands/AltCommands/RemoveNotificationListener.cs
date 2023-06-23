﻿/*
    Copyright(C) 2023  Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using AltTester.AltTesterUnitySDK.Driver.Notifications;

namespace AltTester.AltTesterUnitySDK.Driver.Commands
{
    public class RemoveNotificationListener : AltBaseCommand
    {
        private readonly DeactivateNotification cmdParams;

        public RemoveNotificationListener(IDriverCommunication commHandler, NotificationType notificationType) : base(commHandler)
        {
            this.cmdParams = new DeactivateNotification(notificationType);
        }

        public void Execute()
        {
            this.CommHandler.RemoveNotificationListener(cmdParams.NotificationType);
            this.CommHandler.Send(this.cmdParams);
            var data = this.CommHandler.Recvall<string>(this.cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}
