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

#if UNITY_EDITOR || ALTTESTER
using NLog;
using NLog.Targets;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Driver.Logging
{
    /// <summary> An appender which logs to the unity console. </summary>
    public class UnityTarget : TargetWithLayout
    {
        public UnityTarget(string name)
        {
            this.Name = name;
        }
        /// <inheritdoc />
        protected override void Write(LogEventInfo logEvent)
        {
            string message = this.Layout.Render(logEvent);

            if (logEvent.Level >= LogLevel.Error)
            {
                // everything above or equal to error is an error
                Debug.LogError(message);
            }
            else if (logEvent.Level >= LogLevel.Warn)
            {
                // everything that is a warning up to error is logged as warning
                Debug.LogWarning(message);
            }
            else
            {
                // everything else we'll just log normally
                Debug.Log(message);
            }
        }

    }

}
#endif
