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

namespace AltTester.AltTesterUnitySDK.Driver.Notifications
{
    public class AltLogNotificationResultParams
    {
        public string message;
        public string stackTrace;
        public AltLogLevel level;

        public AltLogNotificationResultParams(string message, string stackTrace, AltLogLevel level)
        {
            this.message = message;
            this.stackTrace = stackTrace;
            this.level = level;
        }
    }
}
