using System;
using System.Collections.Generic;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver.Logging;

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
        public static readonly string VERSION = "1.6.4";

        public IDriverCommunication CommunicationHandler { get { return communicationHandler; } }

        /// <summary>
        /// Initiates AltUnity Driver and begins connection to AltUnity Server
        /// </summary>
        /// <param name="tcp_ip">The ip or hostname  AltUnity Server is listening on.</param>
        /// <param name="tcp_port">The port AltUnity Server is listening on.</param>
        /// <param name="requestSeparator">The separator of command parameters. Must match requestSeparatorString in AltUnity Server </param>
        /// <param name="requestEnding">The ending of the command. Must match requestEnding in AltUnity Server </param>
        /// <param name="logFlag">If true it enables driver commands logging to log file and Unity.</param>
        /// <param name="connectTimeout">The connect timeout.</param>
        public AltUnityDriver(string tcp_ip = "127.0.0.1", int tcp_port = 13000, string requestSeparator = ";", string requestEnding = "&", bool logFlag = false, int connectTimeout = 60)
        {
            if (logFlag)
            {
#if UNITY_EDITOR || ALTUNITYTESTER
                var defaultLevels = new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Unity, AltUnityLogLevel.Debug } };
#else
                var defaultLevels = new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Console, AltUnityLogLevel.Debug } };
#endif
                DriverLogManager.SetupAltUnityDriverLogging(defaultLevels);
            }
            communicationHandler = DriverCommunicationWebSocket.Connect(tcp_ip, tcp_port, connectTimeout);

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
                string message = "Version mismatch. AltUnity Driver version is " + VERSION + ". AltUnity Server version is " + serverVersion + ".";

#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning(message);
#endif
                Console.WriteLine(message);
            }
        }

        public void Stop()
        {
            communicationHandler.Close();
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

        public List<AltUnityObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityFindObjects(communicationHandler, by, value, cameraBy, cameraPath, enabled).Execute();
        }

        public List<AltUnityObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityFindObjectsWhichContain(communicationHandler, by, value, cameraBy, cameraPath, enabled).Execute();
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

        [Obsolete("Use CallStaticMethod overload")]
        public string CallStaticMethod(string typeName, string methodName,
            string parameters, string typeOfParameters = "", string assemblyName = "")

        {
            var paramterTypes = CommandHelpers.ParseParseMethodCallypeOfParameters(typeOfParameters);
            var result = CallStaticMethod<dynamic>(typeName, methodName, CommandHelpers.ParseMethodCallParameters(parameters, paramterTypes), paramterTypes, assemblyName);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);

        }

        public T CallStaticMethod<T>(string typeName, string methodName,
                    string[] parameters, string[] typeOfParameters = null, string assemblyName = "")
        {
            return new AltUnityCallStaticMethod<T>(communicationHandler, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
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
        public void Swipe(AltUnityVector2 start, AltUnityVector2 end, float duration)
        {
            new AltUnitySwipe(communicationHandler, start, end, duration).Execute();
        }
        public void SwipeAndWait(AltUnityVector2 start, AltUnityVector2 end, float duration)
        {
            new AltUnitySwipeAndWait(communicationHandler, start, end, duration).Execute();
        }
        public void MultipointSwipe(AltUnityVector2[] positions, float duration)
        {
            new AltUnityMultipointSwipe(communicationHandler, positions, duration).Execute();
        }
        public void MultipointSwipeAndWait(AltUnityVector2[] positions, float duration)
        {
            new AltUnityMultipointSwipeAndWait(communicationHandler, positions, duration).Execute();
        }
        public void HoldButton(AltUnityVector2 position, float duration)
        {
            Swipe(position, position, duration);
        }
        public void HoldButtonAndWait(AltUnityVector2 position, float duration)
        {
            SwipeAndWait(position, position, duration);
        }

        public void PressKey(AltUnityKeyCode keyCode, float power = 1, float duration = 1)
        {
            new AltUnityPressKey(communicationHandler, keyCode, power, duration).Execute();
        }

        public void PressKeyAndWait(AltUnityKeyCode keyCode, float power = 1, float duration = 1)
        {
            new AltUnityPressKeyAndWait(communicationHandler, keyCode, power, duration).Execute();
        }
        public void MoveMouse(AltUnityVector2 location, float duration = 0)
        {
            new AltUnityMoveMouse(communicationHandler, location, duration).Execute();
        }

        public void MoveMouseAndWait(AltUnityVector2 location, float duration = 0)
        {
            new AltUnityMoveMouseAndWait(communicationHandler, location, duration).Execute();
        }
        public void ScrollMouse(float speed, float duration = 0)
        {
            new AltUnityScrollMouse(communicationHandler, speed, duration).Execute();
        }
        public void ScrollMouseAndWait(float speed, float duration = 0)
        {
            new AltUnityScrollMouseAndWait(communicationHandler, speed, duration).Execute();
        }
        public AltUnityObject TapScreen(float x, float y)
        {
            return new AltUnityTapScreen(communicationHandler, x, y).Execute();
        }
        public void TapCustom(float x, float y, int count, float interval = 0.1f)
        {
            new AltUnityTapCustom(communicationHandler, x, y, count, interval).Execute();
        }
        public void Tilt(AltUnityVector3 acceleration, float duration = 0)
        {
            new AltUnityTilt(communicationHandler, acceleration, duration).Execute();
        }
        public void TiltAndWait(AltUnityVector3 acceleration, float duration = 0)
        {
            new AltUnityTiltAndWait(communicationHandler, acceleration, duration).Execute();
        }

        public List<AltUnityObject> GetAllElements(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityGetAllElements(communicationHandler, cameraBy, cameraPath, enabled).Execute();
        }
        public List<AltUnityObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityGetAllElementsLight(communicationHandler, cameraBy, cameraPath, enabled).Execute();
        }

        public string WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
        {
            return new AltUnityWaitForCurrentSceneToBe(communicationHandler, sceneName, timeout, interval).Execute();
        }

        public AltUnityObject WaitForObject(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObject(communicationHandler, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        public void WaitForObjectNotBePresent(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            new AltUnityWaitForObjectNotBePresent(communicationHandler, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        public AltUnityObject WaitForObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObjectWhichContains(communicationHandler, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        [System.ObsoleteAttribute("Use instead WaitForObject")]
        public AltUnityObject WaitForObjectWithText(By by, string value, string text, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObjectWithText(communicationHandler, by, value, text, cameraBy, cameraPath, enabled, timeout, interval).Execute();
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
    }
}