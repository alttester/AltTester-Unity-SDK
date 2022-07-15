using System;
using System.Collections.Generic;
using System.Threading;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver.Logging;
using Altom.AltUnityDriver.Notifications;

namespace Altom.AltUnityDriver
{
    public enum By
    {
        TAG, LAYER, NAME, COMPONENT, PATH, ID, TEXT
    }

    public class AltUnityDriver
    {
        private static readonly NLog.Logger logger = DriverLogManager.Instance.GetCurrentClassLogger();
        private readonly IDriverCommunication communicationHandler;
        public static readonly string VERSION = "1.7.2";

        public IDriverCommunication CommunicationHandler { get { return communicationHandler; } }

        /// <summary>
        /// Initiates AltUnity Driver and begins connection with the instrumented Unity application through to AltUnity Proxy
        /// </summary>
        /// <param name="host">The ip or hostname  AltUnity Proxy is listening on.</param>
        /// <param name="port">The port AltUnity Proxy is listening on.</param>
        /// <param name="enableLogging">If true it enables driver commands logging to log file and Unity.</param>
        /// <param name="connectTimeout">The connect timeout in seconds.</param>
        public AltUnityDriver(string host = "127.0.0.1", int port = 13000, bool enableLogging = false, int connectTimeout = 60)
        {
#if UNITY_EDITOR || ALTUNITYTESTER
            var defaultLevels = new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Unity, AltUnityLogLevel.Debug } };
#else
                var defaultLevels = new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Console, AltUnityLogLevel.Debug } };
#endif

            DriverLogManager.SetupAltUnityDriverLogging(defaultLevels);

            if (!enableLogging)
                DriverLogManager.StopLogging();

            communicationHandler = new DriverCommunicationWebSocket(host, port, connectTimeout);
            communicationHandler.Connect();

            checkServerVersion();
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

            if (majorServer != majorDriver || minorServer != minorDriver)
            {
                string message = "Version mismatch. AltUnity Driver version is " + VERSION + ". AltUnity Tester version is " + serverVersion + ".";
                logger.Warn(message);
            }
        }

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
            string serverVersion = new AltUnityGetServerVersion(communicationHandler).Execute();
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

        public void LoadScene(string scene, bool loadSingle = true)
        {
            new AltUnityLoadScene(communicationHandler, scene, loadSingle).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void UnloadScene(string scene)
        {
            new AltUnityUnloadScene(communicationHandler, scene).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public List<string> GetAllLoadedScenes()
        {
            var sceneList = new AltUnityGetAllLoadedScenes(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return sceneList;
        }

        public List<AltUnityObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var listOfObjects = new AltUnityFindObjects(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        public List<AltUnityObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var listOfObjects = new AltUnityFindObjectsWhichContain(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        public AltUnityObject FindObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var findObject = new AltUnityFindObject(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return findObject;
        }

        public AltUnityObject FindObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var findObject = new AltUnityFindObjectWhichContains(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return findObject;
        }

        public void SetTimeScale(float timeScale)
        {
            new AltUnitySetTimeScale(communicationHandler, timeScale).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public float GetTimeScale()
        {
            var timeScale = new AltUnityGetTimeScale(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return timeScale;
        }

        public T CallStaticMethod<T>(string typeName, string methodName,
                    object[] parameters, string[] typeOfParameters = null, string assemblyName = "")
        {
            var result = new AltUnityCallStaticMethod<T>(communicationHandler, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return result;
        }

        public T GetStaticProperty<T>(string componentName, string propertyName, string assemblyName, int maxDepth = 2)
        {
            var propertyValue = new AltUnityGetStaticProperty<T>(communicationHandler, componentName, propertyName, assemblyName, maxDepth).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return propertyValue;
        }

        public void DeletePlayerPref()
        {
            new AltUnityDeletePlayerPref(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void DeleteKeyPlayerPref(string keyName)
        {
            new AltUnityDeleteKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void SetKeyPlayerPref(string keyName, int valueName)
        {
            new AltUnitySetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void SetKeyPlayerPref(string keyName, float valueName)
        {
            new AltUnitySetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void SetKeyPlayerPref(string keyName, string valueName)
        {
            new AltUnitySetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public int GetIntKeyPlayerPref(string keyName)
        {
            var keyValue = new AltUnityGetIntKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return keyValue;
        }

        public float GetFloatKeyPlayerPref(string keyName)
        {
            var keyValue = new AltUnityGetFloatKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return keyValue;
        }

        public string GetStringKeyPlayerPref(string keyName)
        {
            var keyValue = new AltUnityGetStringKeyPlayerPref(communicationHandler, keyName).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return keyValue;
        }

        public string GetCurrentScene()
        {
            var sceneName = new AltUnityGetCurrentScene(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return sceneName;
        }

        /// <summary>
        /// Simulates a swipe action between two points.
        /// </summary>
        /// <param name="start">Coordinates of the screen where the swipe begins</param>
        /// <param name="end">Coordinates of the screen where the swipe ends</param>
        /// <param name="duration">The time measured in seconds to move the mouse from start to end location. Defaults to <c>0.1</c>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Swipe(AltUnityVector2 start, AltUnityVector2 end, float duration = 0.1f, bool wait = true)
        {
            new AltUnitySwipe(communicationHandler, start, end, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates a multipoint swipe action.
        /// </summary>
        /// <param name="positions">A list of positions on the screen where the swipe be made.</param>
        /// <param name="duration">The time measured in seconds to swipe from first position to the last position. Defaults to <code>0.1</code>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void MultipointSwipe(AltUnityVector2[] positions, float duration = 0.1f, bool wait = true)
        {
            new AltUnityMultipointSwipe(communicationHandler, positions, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates holding left click button down for a specified amount of time at given coordinates.
        /// </summary>
        /// <param name="coordinates">The coordinates where the button is held down.</param>
        /// <param name="duration">The time measured in seconds to keep the button down.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void HoldButton(AltUnityVector2 coordinates, float duration, bool wait = true)
        {
            Swipe(coordinates, coordinates, duration, wait);
        }

        /// <summary>
        /// Simulates key press action in your game.
        /// </summary>
        /// <param name="keyCode">The key code of the key simulated to be pressed.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void PressKey(AltUnityKeyCode keyCode, float power = 1, float duration = 0.1f, bool wait = true)
        {
            new AltUnityPressKey(communicationHandler, keyCode, power, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void KeyDown(AltUnityKeyCode keyCode, float power = 1)
        {
            new AltUnityKeyDown(communicationHandler, keyCode, power).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void KeyUp(AltUnityKeyCode keyCode)
        {
            new AltUnityKeyUp(communicationHandler, keyCode).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates multiple keys pressed action in your game.
        /// </summary>
        /// <param name="keyCodes">The list of key codes of the keys simulated to be pressed.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        /// <param name="duration">The time measured in seconds from the key press to the key release. Defaults to <c>0.1</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void PressKeys(AltUnityKeyCode[] keyCodes, float power = 1, float duration = 0.1f, bool wait = true)
        {
            new AltUnityPressKeys(communicationHandler, keyCodes, power, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates multiple keys down action in your game.
        /// </summary>
        /// <param name="keyCodes">The key codes of the keys simulated to be down.</param>
        /// <param name="power" >A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Defaults to <c>1</c>.</param>
        public void KeysDown(AltUnityKeyCode[] keyCodes, float power = 1)
        {
            new AltUnityKeysDown(communicationHandler, keyCodes, power).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates multiple keys up action in your game.
        /// </summary>
        /// <param name="keyCodes">The key codes of the keys simulated to be up.</param>
        public void KeysUp(AltUnityKeyCode[] keyCodes)
        {
            new AltUnityKeysUp(communicationHandler, keyCodes).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulate mouse movement in your game.
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="duration">The time measured in seconds to move the mouse from the current mouse position to the set coordinates. Defaults to <c>0.1f</c></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void MoveMouse(AltUnityVector2 coordinates, float duration = 0.1f, bool wait = true)
        {
            new AltUnityMoveMouse(communicationHandler, coordinates, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulate scroll action in your game.
        /// </summary>
        /// <param name="speed">Set how fast to scroll. Positive values will scroll up and negative values will scroll down. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Scroll(float speed = 1, float duration = 0.1f, bool wait = true)
        {
            new AltUnityScroll(communicationHandler, speed, 0, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulate scroll action in your game.
        /// </summary>
        /// <param name="scrollValue">Set how fast to scroll. X is horizontal and Y is vertical. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Scroll(AltUnityVector2 scrollValue, float duration = 0.1f, bool wait = true)
        {
            new AltUnityScroll(communicationHandler, scrollValue.y, scrollValue.x, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Tap at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval between taps in seconds</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Tap(AltUnityVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            new AltUnityTapCoordinates(communicationHandler, coordinates, count, interval, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Click at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count" >Number of clicks.</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Click(AltUnityVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            new AltUnityClickCoordinates(communicationHandler, coordinates, count, interval, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Simulates device rotation action in your game.
        /// </summary>
        /// <param name="acceleration">The linear acceleration of a device.</param>
        /// <param name="duration">How long the rotation will take in seconds. Defaults to <code>0.1<code>.</param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Tilt(AltUnityVector3 acceleration, float duration = 0.1f, bool wait = true)
        {
            new AltUnityTilt(communicationHandler, acceleration, duration, wait).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public List<AltUnityObject> GetAllElements(By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var listOfObjects = new AltUnityGetAllElements(communicationHandler, cameraBy, cameraValue, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        public List<AltUnityObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            var listOfObjects = new AltUnityGetAllElementsLight(communicationHandler, cameraBy, cameraValue, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        public void WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
        {
            new AltUnityWaitForCurrentSceneToBe(communicationHandler, sceneName, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public AltUnityObject WaitForObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            var objectFound = new AltUnityWaitForObject(communicationHandler, by, value, cameraBy, cameraValue, enabled, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return objectFound;
        }

        public void WaitForObjectNotBePresent(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            new AltUnityWaitForObjectNotBePresent(communicationHandler, by, value, cameraBy, cameraValue, enabled, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public AltUnityObject WaitForObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            var objectFound = new AltUnityWaitForObjectWhichContains(communicationHandler, by, value, cameraBy, cameraValue, enabled, timeout, interval).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return objectFound;
        }

        public List<string> GetAllScenes()
        {
            var listOfScenes = new AltUnityGetAllScenes(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfScenes;
        }

        public List<AltUnityObject> GetAllCameras()
        {
            var listOfCameras = new AltUnityGetAllCameras(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfCameras;
        }

        public List<AltUnityObject> GetAllActiveCameras()
        {
            var listOfCameras = new AltUnityGetAllActiveCameras(communicationHandler).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfCameras;
        }

        public AltUnityTextureInformation GetScreenshot(AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            var textureInformation = new AltUnityGetScreenshot(communicationHandler, size, screenShotQuality).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return textureInformation;
        }

        public AltUnityTextureInformation GetScreenshot(int id, AltUnityColor color, float width, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            var textureInformation = new AltUnityGetHightlightObjectScreenshot(communicationHandler, id, color, width, size, screenShotQuality).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return textureInformation;
        }

        public AltUnityTextureInformation GetScreenshot(AltUnityVector2 coordinates, AltUnityColor color, float width, out AltUnityObject selectedObject, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            var textureInformation = new AltUnityGetHightlightObjectFromCoordinatesScreenshot(communicationHandler, coordinates, color, width, size, screenShotQuality).Execute(out selectedObject);
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return textureInformation;
        }

        public void GetPNGScreenshot(string path)
        {
            new AltUnityGetPNGScreenshot(communicationHandler, path).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public List<AltUnityObjectLight> GetAllLoadedScenesAndObjects(bool enabled = true)
        {
            var listOfObjects = new AltUnityGetAllLoadedScenesAndObjects(communicationHandler, enabled).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return listOfObjects;
        }

        public void SetServerLogging(AltUnityLogger logger, AltUnityLogLevel logLevel)
        {
            new AltUnitySetServerLogging(communicationHandler, logger, logLevel).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public int BeginTouch(AltUnityVector2 screenPosition)
        {
            var touchId = new AltUnityBeginTouch(communicationHandler, screenPosition).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return touchId;
        }

        public void MoveTouch(int fingerId, AltUnityVector2 screenPosition)
        {
            new AltUnityMoveTouch(communicationHandler, fingerId, screenPosition).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        public void EndTouch(int fingerId)
        {
            new AltUnityEndTouch(communicationHandler, fingerId).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
        }

        /// <summary>
        /// Retrieves the Unity object at given coordinates.
        /// Uses EventSystem.RaycastAll to find object. If no object is found then it uses UnityEngine.Physics.Raycast and UnityEngine.Physics2D.Raycast and returns the one closer to the camera.
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <returns>The UI object hit by event system Raycast, null otherwise</returns>
        public AltUnityObject FindObjectAtCoordinates(AltUnityVector2 coordinates)
        {
            var objectFound = new AltUnityFindObjectAtCoordinates(communicationHandler, coordinates).Execute();
            communicationHandler.SleepFor(communicationHandler.GetDelayAfterCommand());
            return objectFound;
        }

        public void AddNotificationListener<T>(NotificationType notificationType, Action<T> callback, bool overwrite)
        {
            new AddNotificationListener<T>(communicationHandler, notificationType, callback, overwrite).Execute();
        }

        public void RemoveNotificationListener(NotificationType notificationType)
        {
            new RemoveNotificationListener(communicationHandler, notificationType).Execute();
        }
    }
}
