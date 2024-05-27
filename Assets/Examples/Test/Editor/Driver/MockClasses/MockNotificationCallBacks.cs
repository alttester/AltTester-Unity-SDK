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

namespace AltTester.AltTesterUnitySDK.Driver.MockClasses
{
    internal class MockNotificationCallBacks : INotificationCallbacks
    {
        public static string LastSceneLoaded = "";
        public static string LastSceneUnloaded = "";
        public static string LogMessage = "";
        public static AltLogLevel LogLevel = AltLogLevel.Error;
        public static string StackTrace = "";
        public static bool ApplicationPaused = false;
        public void SceneLoadedCallback(AltLoadSceneNotificationResultParams altLoadSceneNotificationResultParams)
        {
            LastSceneLoaded = altLoadSceneNotificationResultParams.sceneName;
        }
        public void SceneUnloadedCallback(string sceneName)
        {
            LastSceneUnloaded = sceneName;
        }
        public void LogCallback(AltLogNotificationResultParams altLogNotificationResultParams)
        {
            LogMessage = altLogNotificationResultParams.message;
            LogLevel = altLogNotificationResultParams.level;
            StackTrace = altLogNotificationResultParams.stackTrace;
        }
        public void ApplicationPausedCallback(bool applicationPaused)
        {
            ApplicationPaused = applicationPaused;
        }
    }
}
