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

using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Notifications;
using AltTester.AltTesterUnitySDK.Commands;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Notification
{
    public class AltLogNotification : BaseNotification
    {
        public AltLogNotification(ICommandHandler commandHandler, bool isOn) : base(commandHandler)
        {
            Application.logMessageReceived -= onLogReceived;

            if (isOn)
            {
                Application.logMessageReceived += onLogReceived;
            }
        }

        static void onLogReceived(string message, string stackTrace, LogType type)
        {
            var data = new AltLogNotificationResultParams(message, stackTrace, logTypeToLogLevel(type));
            SendNotification(data, "logNotification");
        }

        private static AltLogLevel logTypeToLogLevel(LogType type)
        {
            if ((int)type == (int)LogType.Error || (int)type == (int)LogType.Exception)
                return AltLogLevel.Error;
            else if ((int)type == (int)LogType.Assert || (int)type == (int)LogType.Log)
                return AltLogLevel.Debug;
            return AltLogLevel.Warn;
        }
    }
}
