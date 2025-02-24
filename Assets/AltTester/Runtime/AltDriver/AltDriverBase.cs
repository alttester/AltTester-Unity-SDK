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

using System;
using System.Collections.Generic;
using System.Threading;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Driver.Communication;
using AltTester.AltTesterUnitySDK.Driver.Logging;
using AltTester.AltTesterUnitySDK.Driver.Notifications;

namespace AltTester.AltTesterUnitySDK.Driver
{
    public enum By
    {
        TAG, LAYER, NAME, COMPONENT, PATH, ID, TEXT
    }

    public class AltDriverBase
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        protected readonly IDriverCommunication communicationHandler;
        private static object driverLock = new object();
        public static readonly string VERSION = "2.2.2";

        public IDriverCommunication CommunicationHandler { get { return communicationHandler; } }

        /// <summary>
        /// Initiates AltDriver and begins connection with the instrumented Unity application through to AltServer.
        /// </summary>
        /// <param name="host">The IP or hostname AltTester® Server is listening on.</param>
        /// <param name="port">The port AltTester® Server is listening on.</param>
        /// <param name="enableLogging">If true it enables driver commands logging to log file and Unity.</param>
        /// <param name="connectTimeout">The connect timeout in seconds.</param>
        /// <param name="appName">The name of the Unity application.</param>
        public AltDriverBase(string host = "127.0.0.1", int port = 13000, string appName = "__default__", bool enableLogging = false, int connectTimeout = 60, string platform = "unknown", string platformVersion = "unknown", string deviceInstanceId = "unknown", string appId = "unknown", string driverType = "SDK")
        {
            lock (driverLock)
            {
#if UNITY_EDITOR || ALTTESTER
                var defaultLevels = new Dictionary<AltLogger, AltLogLevel> { { AltLogger.File, AltLogLevel.Debug }, { AltLogger.Unity, AltLogLevel.Debug } };
#else
                var defaultLevels = new Dictionary<AltLogger, AltLogLevel> { { AltLogger.File, AltLogLevel.Debug }, { AltLogger.Console, AltLogLevel.Debug } };
#endif


                if (enableLogging)
                {
                    DriverLogManager.SetupAltDriverLogging(defaultLevels);
                }
                else
                {
                    DriverLogManager.StopLogging();
                }

                logger.Debug(
                    "Connecting to AltTester(R) on host: '{0}', port: '{1}', appName: '{2}', platform: '{3}', platformVersion: '{4}', deviceInstanceId: '{5}' and driverType: '{6}'.",
                    host,
                    port,
                    appName,
                    platform,
                    platformVersion,
                    deviceInstanceId,
                    driverType
                );
                while (true)
                {
                    communicationHandler = new DriverCommunicationHandler(host, port, connectTimeout, appName, platform, platformVersion, deviceInstanceId, appId, driverType);
                    communicationHandler.Connect();
                    try
                    {
                        checkServerVersion();
                        break;
                    }
                    catch (NullReferenceException)//There is a strange situation when sometimes checkServerVersion throws that command params is null. I investigated but didn't find the cause.
                    {
                        communicationHandler.Close();
                    }
                }
            }
        }

        private void splitVersion(string version, out string major, out string minor)
        {
            var parts = version.Split(new[] { "." }, StringSplitOptions.None);
            major = parts[0];
            minor = parts.Length > 1 ? parts[1] : string.Empty;
        }

        private void checkServerVersion()
        {
            string serverVersion = GetServerVersion();

            string majorServer;
            string majorDriver;
            string minorDriver;
            string minorServer;

            splitVersion(serverVersion, out majorServer, out minorServer);
            splitVersion(VERSION, out majorDriver, out minorDriver);

            int serverMajor, serverMinor;

            serverMajor = int.Parse(majorServer);
            serverMinor = int.Parse(minorServer);

            bool isSupported =
        (serverMajor == 2 && serverMinor == 2) || // Server version 2.2.x
        (serverMajor == 1 && serverMinor == 0);    // Server version 1.0.0

            if (!isSupported)
            {
                string message = $"Version mismatch. AltDriver version is {VERSION}. AltTester(R) version is {serverVersion}.";
                logger.Warn(message);
            }
        }


        ////////////////////////////////////////////
        ///
        /// Common methods for all drivers, they are public and can be used directly in the derived classes
        /// 
        ////////////////////////////////////////////

        public void Stop()
        {
            communicationHandler.Close();
        }

        public void SetCommandResponseTimeout(int commandTimeout)
        {
            communicationHandler.SetCommandTimeout(commandTimeout);
        }

        public void SetDelayAfterCommand(float delay)
        {
            communicationHandler.SetDelayAfterCommand(delay);
        }

        public float GetDelayAfterCommand()
        {
            return communicationHandler.GetDelayAfterCommand();
        }

        public string GetServerVersion()
        {
            string serverVersion = new AltGetServerVersion(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return serverVersion;
        }

        public void SetLogging(bool enableLogging)
        {
            if (enableLogging)
                DriverLogManager.ResumeLogging();
            else
                DriverLogManager.StopLogging();
        }
        
        public void KeyUp(AltKeyCode keyCode)
        {
            AltKeyCode[] keyCodes = { keyCode };
            KeysUp(keyCodes);
        }

        /// <summary>
        /// Simulates multiple keys up action in your app.
        /// </summary>
        /// <param name="keyCodes">The key codes of the keys simulated to be up.</param>
        public void KeysUp(AltKeyCode[] keyCodes)
        {
            new AltKeysUp(communicationHandler, keyCodes).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public AltTextureInformation GetScreenshot(AltVector2 size = default(AltVector2), int screenShotQuality = 100)
        {
            var textureInformation = new AltGetScreenshot(communicationHandler, size, screenShotQuality).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return textureInformation;
        }

        public AltTextureInformation GetScreenshot(int id, AltColor color, float width, AltVector2 size = default(AltVector2), int screenShotQuality = 100)
        {
            var textureInformation = new AltGetHighlightObjectScreenshot(communicationHandler, id, color, width, size, screenShotQuality).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return textureInformation;
        }

        public AltTextureInformation GetScreenshot(AltVector2 coordinates, AltColor color, float width, out AltObject selectedObject, AltVector2 size = default(AltVector2), int screenShotQuality = 100)
        {
            var textureInformation = new AltGetHighlightObjectFromCoordinatesScreenshot(communicationHandler, coordinates, color, width, size, screenShotQuality).Execute(out selectedObject);
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return textureInformation;
        }

        public void GetPNGScreenshot(string path)
        {
            new AltGetPNGScreenshot(communicationHandler, path).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public List<AltObjectLight> GetAllLoadedScenesAndObjects(bool enabled = true)
        {
            var listOfObjects = new AltGetAllLoadedScenesAndObjects(communicationHandler, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        public void SetServerLogging(AltLogger logger, AltLogLevel logLevel)
        {
            new AltSetServerLogging(communicationHandler, logger, logLevel).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }


        public void AddNotificationListener<T>(NotificationType notificationType, Action<T> callback, bool overwrite)
        {
            new AddNotificationListener<T>(communicationHandler, notificationType, callback, overwrite).Execute();
        }

        public void RemoveNotificationListener(NotificationType notificationType)
        {
            new RemoveNotificationListener(communicationHandler, notificationType).Execute();
        }

        ////////////////////////////////////////////
        ///
        /// Protected methods that need to be implemented in the derived classes
        /// 
        ////////////////////////////////////////////


        protected void ResetInput()
        {
            new AltResetInput(communicationHandler).Execute();
        }

        protected void LoadScene(string scene, bool loadSingle = true)
        {
            new AltLoadScene(communicationHandler, scene, loadSingle).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void UnloadScene(string scene)
        {
            new AltUnloadScene(communicationHandler, scene).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected List<string> GetAllLoadedScenes()
        {
            var sceneList = new AltGetAllLoadedScenes(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return sceneList;
        }

        protected string GetCurrentScene()
        {
            var sceneName = new AltGetCurrentScene(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return sceneName;
        }

        protected void WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
        {
            new AltWaitForCurrentSceneToBe(communicationHandler, sceneName, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void SetTimeScale(float timeScale)
        {
            new AltSetTimeScale(communicationHandler, timeScale).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected float GetTimeScale()
        {
            var timeScale = new AltGetTimeScale(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return timeScale;
        }


        protected void DeletePlayerPref()
        {
            new AltDeletePlayerPref(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void DeleteKeyPlayerPref(string keyName)
        {
            new AltDeleteKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void SetKeyPlayerPref(string keyName, int valueName)
        {
            new AltSetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void SetKeyPlayerPref(string keyName, float valueName)
        {
            new AltSetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void SetKeyPlayerPref(string keyName, string valueName)
        {
            new AltSetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected int GetIntKeyPlayerPref(string keyName)
        {
            var keyValue = new AltGetIntKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return keyValue;
        }

        protected float GetFloatKeyPlayerPref(string keyName)
        {
            var keyValue = new AltGetFloatKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return keyValue;
        }

        protected string GetStringKeyPlayerPref(string keyName)
        {
            var keyValue = new AltGetStringKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return keyValue;
        }

        protected List<AltObject> FindObjects(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var listOfObjects = new AltFindObjects(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        protected List<AltObject> FindObjectsWhichContain(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var listOfObjects = new AltFindObjectsWhichContain(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        protected AltObject FindObject(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var findObject = new AltFindObject(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return findObject;
        }

        protected AltObject FindObjectWhichContains(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var findObject = new AltFindObjectWhichContains(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return findObject;
        }

        protected T CallStaticMethod<T>(string typeName, string methodName, string assemblyName,
                    object[] parameters, string[] typeOfParameters = null)
        {
            var result = new AltCallStaticMethod<T>(communicationHandler, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return result;
        }

        protected T GetStaticProperty<T>(string componentName, string propertyName, string assemblyName, int maxDepth = 2)
        {
            var propertyValue = new AltGetStaticProperty<T>(communicationHandler, componentName, propertyName, assemblyName, maxDepth).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return propertyValue;
        }

        protected void SetStaticProperty(string componentName, string propertyName, string assemblyName, object updatedProperty)
        {
            new AltSetStaticProperty(communicationHandler, componentName, propertyName, assemblyName, updatedProperty).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates a swipe action between two points.
        /// </summary>
        /// <param name="start">Coordinates of the screen where the swipe begins</param>
        /// <param name="end">Coordinates of the screen where the swipe ends</param>
        /// <param name="duration">The time measured in seconds to move the mouse from start to end location. Defaults to <c>0.1</c>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void Swipe(AltVector2 start, AltVector2 end, float duration = 0.1f, bool wait = true)
        {
            new AltSwipe(communicationHandler, start, end, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates a multipoint swipe action.
        /// </summary>
        /// <param name="positions">A list of positions on the screen where the swipe be made.</param>
        /// <param name="duration">The time measured in seconds to swipe from first position to the last position. Defaults to <code>0.1</code>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void MultipointSwipe(AltVector2[] positions, float duration = 0.1f, bool wait = true)
        {
            new AltMultipointSwipe(communicationHandler, positions, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates holding left click button down for a specified amount of time at given coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates where the button is held down.</param>
        /// <param name="duration">The time measured in seconds to keep the button down.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void HoldButton(AltVector2 coordinates, float duration, bool wait = true)
        {
            Swipe(coordinates, coordinates, duration, wait);
        }

        /// <summary>
        /// Simulates key press action in your app.
        /// </summary>
        /// <param name="keyCode">The key code of the key simulated to be pressed.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void PressKey(AltKeyCode keyCode, float power = 1, float duration = 0.1f, bool wait = true)
        {
            AltKeyCode[] keyCodes = { keyCode };
            PressKeys(keyCodes, power, duration, wait);
        }

        /// <summary>
        /// Simulates multiple keys pressed action in your app.
        /// </summary>
        /// <param name="keyCodes">The list of key codes of the keys simulated to be pressed.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void PressKeys(AltKeyCode[] keyCodes, float power = 1, float duration = 0.1f, bool wait = true)
        {
            new AltPressKeys(communicationHandler, keyCodes, power, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void KeyDown(AltKeyCode keyCode, float power = 1)
        {
            AltKeyCode[] keyCodes = { keyCode };
            KeysDown(keyCodes, power);
        }

        /// <summary>
        /// Simulates multiple keys down action in your app.
        /// </summary>
        /// <param name="keyCodes">The key codes of the keys simulated to be down.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        protected void KeysDown(AltKeyCode[] keyCodes, float power = 1)
        {
            new AltKeysDown(communicationHandler, keyCodes, power).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulate mouse movement in your app.
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="duration">The time measured in seconds to move the mouse from the current mouse position to the set coordinates. Defaults to <c>0.1f</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void MoveMouse(AltVector2 coordinates, float duration = 0.1f, bool wait = true)
        {
            new AltMoveMouse(communicationHandler, coordinates, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulate scroll action in your app.
        /// </summary>
        /// <param name="speed">Set how fast to scroll. Positive values will scroll up and negative values will scroll down. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void Scroll(float speed = 1, float duration = 0.1f, bool wait = true)
        {
            new AltScroll(communicationHandler, speed, 0, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulate scroll action in your app.
        /// </summary>
        /// <param name="scrollValue">Set how fast to scroll. X is horizontal and Y is vertical. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void Scroll(AltVector2 scrollValue, float duration = 0.1f, bool wait = true)
        {
            new AltScroll(communicationHandler, scrollValue.y, scrollValue.x, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Tap at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval between taps in seconds</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void Tap(AltVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            new AltTapCoordinates(communicationHandler, coordinates, count, interval, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Click at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count" >Number of clicks.</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Click(AltVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            new AltClickCoordinates(communicationHandler, coordinates, count, interval, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates device rotation action in your app.
        /// </summary>
        /// <param name="acceleration">The linear acceleration of a device.</param>
        /// <param name="duration">How long the rotation will take in seconds. Defaults to <code>0.1<code>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        protected void Tilt(AltVector3 acceleration, float duration = 0.1f, bool wait = true)
        {
            new AltTilt(communicationHandler, acceleration, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected List<AltObject> GetAllElements(AltBy cameraAltBy = null, bool enabled = true)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var listOfObjects = new AltGetAllElements(communicationHandler, cameraAltBy.By, cameraAltBy.Value, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        protected List<AltObjectLight> GetAllElementsLight(AltBy cameraAltBy = null, bool enabled = true)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var listOfObjects = new AltGetAllElementsLight(communicationHandler, cameraAltBy.By, cameraAltBy.Value, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        protected AltObject WaitForObject(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var objectFound = new AltWaitForObject(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return objectFound;
        }

        protected void WaitForObjectNotBePresent(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            new AltWaitForObjectNotBePresent(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected AltObject WaitForObjectWhichContains(AltBy altBy, AltBy cameraAltBy = null, bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            cameraAltBy ??= new AltBy(By.NAME, "");
            var objectFound = new AltWaitForObjectWhichContains(communicationHandler, altBy.By, altBy.Value, cameraAltBy.By, cameraAltBy.Value, enabled, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return objectFound;
        }

        protected List<string> GetAllScenes()
        {
            var listOfScenes = new AltGetAllScenes(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfScenes;
        }

        protected List<AltObject> GetAllCameras()
        {
            var listOfCameras = new AltGetAllCameras(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfCameras;
        }

        protected List<AltObject> GetAllActiveCameras()
        {
            var listOfCameras = new AltGetAllActiveCameras(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfCameras;
        }

        protected AltVector2 GetApplicationScreenSize()
        {
            var applicationScreenSize = new AltGetApplicationScreenSize(communicationHandler).Execute();
            return applicationScreenSize;
        }

        protected int BeginTouch(AltVector2 screenPosition)
        {
            var touchId = new AltBeginTouch(communicationHandler, screenPosition).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return touchId;
        }

        protected void MoveTouch(int fingerId, AltVector2 screenPosition)
        {
            new AltMoveTouch(communicationHandler, fingerId, screenPosition).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected void EndTouch(int fingerId)
        {
            new AltEndTouch(communicationHandler, fingerId).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        protected AltObject FindObjectAtCoordinates(AltVector2 coordinates)
        {
            var objectFound = new AltFindObjectAtCoordinates(communicationHandler, coordinates).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return objectFound;
        }
    }
}
