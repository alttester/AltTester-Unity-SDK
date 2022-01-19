using System;
using System.Collections.Generic;
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
        public static readonly string VERSION = "1.7.0";

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
            if (enableLogging)
            {
#if UNITY_EDITOR || ALTUNITYTESTER
                var defaultLevels = new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Unity, AltUnityLogLevel.Debug } };
#else
                var defaultLevels = new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Console, AltUnityLogLevel.Debug } };
#endif
                DriverLogManager.SetupAltUnityDriverLogging(defaultLevels);
            }

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
            string serverVersion;
            try
            {
                serverVersion = GetServerVersion();
            }
            catch (UnknownErrorException)
            {
                serverVersion = "<=1.5.3";
            }
            catch (AltUnityRecvallMessageFormatException)
            {
                serverVersion = "<=1.5.7";
            }
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

        public string GetServerVersion()
        {
            return new AltUnityGetServerVersion(communicationHandler).Execute();
        }
        public void LoadScene(string scene, bool loadSingle = true)
        {
            new AltUnityLoadScene(communicationHandler, scene, loadSingle).Execute();
        }
        public void UnloadScene(string scene)
        {
            new AltUnityUnloadScene(communicationHandler, scene).Execute();
        }
        public List<string> GetAllLoadedScenes()
        {
            return new AltUnityGetAllLoadedScenes(communicationHandler).Execute();
        }

        public List<AltUnityObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityFindObjects(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
        }

        public List<AltUnityObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityFindObjectsWhichContain(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
        }

        public AltUnityObject FindObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityFindObject(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
        }

        public AltUnityObject FindObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityFindObjectWhichContains(communicationHandler, by, value, cameraBy, cameraValue, enabled).Execute();
        }

        public void SetTimeScale(float timeScale)
        {
            new AltUnitySetTimeScale(communicationHandler, timeScale).Execute();
        }
        public float GetTimeScale()
        {
            return new AltUnityGetTimeScale(communicationHandler).Execute();
        }

        public T CallStaticMethod<T>(string typeName, string methodName,
                    object[] parameters, string[] typeOfParameters = null, string assemblyName = "")
        {
            return new AltUnityCallStaticMethod<T>(communicationHandler, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
        }

        public T GetStaticProperty<T>(string componentName, string propertyName, string assemblyName, int maxDepth = 2)
        {
            return new AltUnityGetStaticProperty<T>(communicationHandler, componentName, propertyName, assemblyName, maxDepth).Execute();
        }

        public void DeletePlayerPref()
        {
            new AltUnityDeletePlayerPref(communicationHandler).Execute();
        }
        public void DeleteKeyPlayerPref(string keyName)
        {
            new AltUnityDeleteKeyPlayerPref(communicationHandler, keyName).Execute();
        }
        public void SetKeyPlayerPref(string keyName, int valueName)
        {
            new AltUnitySetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
        }
        public void SetKeyPlayerPref(string keyName, float valueName)
        {
            new AltUnitySetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
        }
        public void SetKeyPlayerPref(string keyName, string valueName)
        {
            new AltUnitySetKeyPLayerPref(communicationHandler, keyName, valueName).Execute();
        }
        public int GetIntKeyPlayerPref(string keyName)
        {
            return new AltUnityGetIntKeyPlayerPref(communicationHandler, keyName).Execute();
        }
        public float GetFloatKeyPlayerPref(string keyName)
        {
            return new AltUnityGetFloatKeyPlayerPref(communicationHandler, keyName).Execute();
        }
        public string GetStringKeyPlayerPref(string keyName)
        {
            return new AltUnityGetStringKeyPlayerPref(communicationHandler, keyName).Execute();
        }
        public string GetCurrentScene()
        {
            return new AltUnityGetCurrentScene(communicationHandler).Execute();
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
        }
        public void KeyDown(AltUnityKeyCode keyCode, float power = 1)
        {
            new AltUnityKeyDown(communicationHandler, keyCode, power).Execute();
        }
        public void KeyUp(AltUnityKeyCode keyCode)
        {
            new AltUnityKeyUp(communicationHandler, keyCode).Execute();
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
        }
        /// <summary>
        /// Simulate scroll action in your game.
        /// </summary>
        /// <param name="speed">Set how fast to scroll. Positive values will scroll up and negative values will scroll down. Defaults to <code> 1 </code></param>
        /// <param name="duration">The duration of the scroll in seconds. Defaults to <code> 0.1 </code></param>
        /// <param name="wait">If set wait for command to finish. Defaults to <c>True</c>.</param>
        public void Scroll(float speed = 1, float duration = 0.1f, bool wait = true)
        {
            new AltUnityScroll(communicationHandler, speed, duration, wait).Execute();
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
        }

        public List<AltUnityObject> GetAllElements(By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityGetAllElements(communicationHandler, cameraBy, cameraValue, enabled).Execute();
        }
        public List<AltUnityObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityGetAllElementsLight(communicationHandler, cameraBy, cameraValue, enabled).Execute();
        }

        public void WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
        {
            new AltUnityWaitForCurrentSceneToBe(communicationHandler, sceneName, timeout, interval).Execute();
        }

        public AltUnityObject WaitForObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObject(communicationHandler, by, value, cameraBy, cameraValue, enabled, timeout, interval).Execute();
        }

        public void WaitForObjectNotBePresent(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            new AltUnityWaitForObjectNotBePresent(communicationHandler, by, value, cameraBy, cameraValue, enabled, timeout, interval).Execute();
        }

        public AltUnityObject WaitForObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObjectWhichContains(communicationHandler, by, value, cameraBy, cameraValue, enabled, timeout, interval).Execute();
        }

        public List<string> GetAllScenes()
        {
            return new AltUnityGetAllScenes(communicationHandler).Execute();
        }
        public List<AltUnityObject> GetAllCameras()
        {
            return new AltUnityGetAllCameras(communicationHandler).Execute();
        }

        public List<AltUnityObject> GetAllActiveCameras()
        {
            return new AltUnityGetAllActiveCameras(communicationHandler).Execute();
        }
        public AltUnityTextureInformation GetScreenshot(AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            return new AltUnityGetScreenshot(communicationHandler, size, screenShotQuality).Execute();
        }
        public AltUnityTextureInformation GetScreenshot(int id, AltUnityColor color, float width, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            return new AltUnityGetHightlightObjectScreenshot(communicationHandler, id, color, width, size, screenShotQuality).Execute();
        }
        public AltUnityTextureInformation GetScreenshot(AltUnityVector2 coordinates, AltUnityColor color, float width, out AltUnityObject selectedObject, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            return new AltUnityGetHightlightObjectFromCoordinatesScreenshot(communicationHandler, coordinates, color, width, size, screenShotQuality).Execute(out selectedObject);
        }
        public void GetPNGScreenshot(string path)
        {
            new AltUnityGetPNGScreenshot(communicationHandler, path).Execute();
        }
        public List<AltUnityObjectLight> GetAllLoadedScenesAndObjects(bool enabled = true)
        {
            return new AltUnityGetAllLoadedScenesAndObjects(communicationHandler, enabled).Execute();
        }

        public void SetServerLogging(AltUnityLogger logger, AltUnityLogLevel logLevel)
        {
            new AltUnitySetServerLogging(communicationHandler, logger, logLevel).Execute();
        }
        public int BeginTouch(AltUnityVector2 screenPosition)
        {
            return new AltUnityBeginTouch(communicationHandler, screenPosition).Execute();
        }
        public void MoveTouch(int fingerId, AltUnityVector2 screenPosition)
        {
            new AltUnityMoveTouch(communicationHandler, fingerId, screenPosition).Execute();
        }
        public void EndTouch(int fingerId)
        {
            new AltUnityEndTouch(communicationHandler, fingerId).Execute();
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