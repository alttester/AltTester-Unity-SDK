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

using System.Globalization;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;
using Newtonsoft.Json;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class BaseNotification
    {
        private static ICommandHandler commandHandler;

        public BaseNotification(ICommandHandler commandHandlerParam)
        {
            commandHandler = commandHandlerParam;
        }

        protected static void SendNotification<T>(T data, string commandName)
        {
            var cmdResponse = new CommandResponse
            {
                commandName = commandName,
                messageId = null,
                data = JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Culture = CultureInfo.InvariantCulture
                }),
                error = null,
                isNotification = true
            };

            var notification = JsonConvert.SerializeObject(cmdResponse, new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            });
            commandHandler.Send(notification);

        }
    }
}