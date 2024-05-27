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
using AltTester.AltTesterUnitySDK.Driver.Logging;

namespace AltTester.AltTesterUnitySDK.Driver.Notifications
{
    public class BaseNotificationCallBacks : INotificationCallbacks
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        public void SceneLoadedCallback(AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Scene {0} was loaded {1}", altLoadSceneNotificationResultParams.sceneName, altLoadSceneNotificationResultParams.loadSceneMode.ToString()));
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Scene {0} was unloaded", sceneName));
        }
        public void LogCallback(AltLogNotificationResultParams altLogNotificationResultParams)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Log of type {0} with message {1} was received", altLogNotificationResultParams.level, altLogNotificationResultParams.message));
        }
        public void ApplicationPausedCallback(bool applicationPaused)
        {
            logger.Log(NLog.LogLevel.Info, String.Format("Application paused: {0}", applicationPaused));
        }
    }
}
