using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
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
        private readonly TcpClient tcpClient;
        private readonly SocketSettings socketSettings;
        public static readonly string VERSION = "1.6.6";

        public TcpClient TcpClient { get { return tcpClient; } }
        public SocketSettings SocketSettings { get { return socketSettings; } }

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

            int retryPeriod = 5;
            while (connectTimeout > 0)
            {
                try
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(tcp_ip, tcp_port);
                    tcpClient.SendTimeout = 5000;
                    tcpClient.ReceiveTimeout = 5000;
                    var altSocket = new AltSocket.Socket(tcpClient.Client);
                    socketSettings = new SocketSettings(altSocket, requestSeparator, requestEnding);
                    checkServerVersion();
                    break;
                }
                catch (Exception ex)
                {
                    if (tcpClient != null)
                        Stop();

                    string errorMessage = "Trying to reach AltUnity Server at port" + tcp_port + ",retrying in " + retryPeriod + " (timing out in " + connectTimeout + " secs)...";
                    Console.WriteLine(errorMessage);
#if UNITY_EDITOR
                    UnityEngine.Debug.Log(errorMessage);
#endif

                    connectTimeout -= retryPeriod;
                    if (connectTimeout <= 0)
                    {
                        throw new Exception("Could not create connection to " + tcp_ip + ":" + tcp_port, ex);
                    }
                    Thread.Sleep(retryPeriod * 1000);
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
            new AltUnityStopCommand(socketSettings).Execute();
            tcpClient.Close();
        }

        public string GetServerVersion()
        {
            return new AltUnityGetServerVersion(socketSettings).Execute();
        }
        public void LoadScene(string scene, bool loadSingle = true)
        {
            new AltUnityLoadScene(socketSettings, scene, loadSingle).Execute();
        }
        public void UnloadScene(string scene)
        {
            new AltUnityUnloadScene(socketSettings, scene).Execute();
        }
        public List<string> GetAllLoadedScenes()
        {
            return new AltUnityGetAllLoadedScenes(socketSettings).Execute();
        }

        public List<AltUnityObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityFindObjects(socketSettings, by, value, cameraBy, cameraPath, enabled).Execute();
        }

        public List<AltUnityObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityFindObjectsWhichContain(socketSettings, by, value, cameraBy, cameraPath, enabled).Execute();
        }

        public AltUnityObject FindObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityFindObject(socketSettings, by, value, cameraBy, cameraValue, enabled).Execute();
        }

        public AltUnityObject FindObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
        {
            return new AltUnityFindObjectWhichContains(socketSettings, by, value, cameraBy, cameraValue, enabled).Execute();
        }

        public void SetTimeScale(float timeScale)
        {
            new AltUnitySetTimeScale(socketSettings, timeScale).Execute();
        }
        public float GetTimeScale()
        {
            return new AltUnityGetTimeScale(socketSettings).Execute();
        }
        [Obsolete("Use instead CallStaticMethod")]
        public string CallStaticMethods(string typeName, string methodName,
            string parameters, string typeOfParameters = "", string assemblyName = "")
        {
            return new AltUnityCallStaticMethod(socketSettings, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
        }
        public string CallStaticMethod(string typeName, string methodName,
            string parameters, string typeOfParameters = "", string assemblyName = "")
        {
            return new AltUnityCallStaticMethod(socketSettings, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
        }
        public void DeletePlayerPref()
        {
            new AltUnityDeletePlayerPref(socketSettings).Execute();
        }
        public void DeleteKeyPlayerPref(string keyName)
        {
            new AltUnityDeleteKeyPlayerPref(socketSettings, keyName).Execute();
        }
        public void SetKeyPlayerPref(string keyName, int valueName)
        {
            new AltUnitySetKeyPLayerPref(socketSettings, keyName, valueName).Execute();
        }
        public void SetKeyPlayerPref(string keyName, float valueName)
        {
            new AltUnitySetKeyPLayerPref(socketSettings, keyName, valueName).Execute();
        }
        public void SetKeyPlayerPref(string keyName, string valueName)
        {
            new AltUnitySetKeyPLayerPref(socketSettings, keyName, valueName).Execute();
        }
        public int GetIntKeyPlayerPref(string keyName)
        {
            return new AltUnityGetIntKeyPLayerPref(socketSettings, keyName).Execute();
        }
        public float GetFloatKeyPlayerPref(string keyName)
        {
            return new AltUnityGetFloatKeyPlayerPref(socketSettings, keyName).Execute();
        }
        public string GetStringKeyPlayerPref(string keyName)
        {
            return new AltUnityGetStringKeyPlayerPref(socketSettings, keyName).Execute();
        }
        public string GetCurrentScene()
        {
            return new AltUnityGetCurrentScene(socketSettings).Execute();
        }
        public void Swipe(AltUnityVector2 start, AltUnityVector2 end, float duration)
        {
            new AltUnitySwipe(socketSettings, start, end, duration).Execute();
        }
        public void SwipeAndWait(AltUnityVector2 start, AltUnityVector2 end, float duration)
        {
            new AltUnitySwipeAndWait(socketSettings, start, end, duration).Execute();
        }
        public void MultipointSwipe(AltUnityVector2[] positions, float duration)
        {
            new AltUnityMultipointSwipe(socketSettings, positions, duration).Execute();
        }
        public void MultipointSwipeAndWait(AltUnityVector2[] positions, float duration)
        {
            new AltUnityMultipointSwipeAndWait(socketSettings, positions, duration).Execute();
        }
        public void HoldButton(AltUnityVector2 position, float duration)
        {
            Swipe(position, position, duration);
        }
        public void HoldButtonAndWait(AltUnityVector2 position, float duration)
        {
            SwipeAndWait(position, position, duration);
        }
        [Obsolete("Use PressKey(AltUnityKeyCode, float, float) instead.")]
        public void PressKey(String keyName, float power = 1, float duration = 1)
        {
            new AltUnityPressKey(socketSettings, keyName, power, duration).Execute();
        }
        public void PressKey(AltUnityKeyCode keyCode, float power = 1, float duration = 1)
        {
            new AltUnityPressKey(socketSettings, keyCode, power, duration).Execute();
        }
        public void KeyDown(AltUnityKeyCode keyCode, float power = 1)
        {
            new AltUnityKeyDown(socketSettings, keyCode, power).Execute();
        }
        public void KeyUp(AltUnityKeyCode keyCode)
        {
            new AltUnityKeyUp(socketSettings, keyCode).Execute();
        }
        [Obsolete("Use PressKeyAndWait(AltUnityKeyCode, float, float) instead.")]
        public void PressKeyAndWait(String keyName, float power = 1, float duration = 1)
        {
            new AltUnityPressKeyAndWait(socketSettings, keyName, power, duration).Execute();
        }
        public void PressKeyAndWait(AltUnityKeyCode keyCode, float power = 1, float duration = 1)
        {
            new AltUnityPressKeyAndWait(socketSettings, keyCode, power, duration).Execute();
        }
        public void MoveMouse(AltUnityVector2 location, float duration = 0)
        {
            new AltUnityMoveMouse(socketSettings, location, duration).Execute();
        }
        public void MoveMouseAndWait(AltUnityVector2 location, float duration = 0)
        {
            new AltUnityMoveMouseAndWait(socketSettings, location, duration).Execute();
        }
        public void ScrollMouse(float speed, float duration = 0)
        {
            new AltUnityScrollMouse(socketSettings, speed, duration).Execute();
        }
        public void ScrollMouseAndWait(float speed, float duration = 0)
        {
            new AltUnityScrollMouseAndWait(socketSettings, speed, duration).Execute();
        }
        [Obsolete("Use Tap")]
        public AltUnityObject TapScreen(float x, float y)
        {
            return new AltUnityTapScreen(socketSettings, x, y).Execute();
        }
        [Obsolete("Use Tap")]
        public void TapCustom(float x, float y, int count, float interval = 0.1f)
        {
            new AltUnityTapCustom(socketSettings, x, y, count, interval).Execute();
        }

        /// <summary>
        /// Tap at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count">Number of taps</param>
        /// <param name="interval">Interval between taps in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        public void Tap(AltUnityVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            new AltUnityTapCoordinates(socketSettings, coordinates, count, interval, wait).Execute();
        }

        /// <summary>
        /// Click at screen coordinates
        /// </summary>
        /// <param name="coordinates">The screen coordinates</param>
        /// <param name="count" >Number of clicks.</param>
        /// <param name="interval">Interval between clicks in seconds</param>
        /// <param name="wait">Wait for command to finish</param>
        public void Click(AltUnityVector2 coordinates, int count = 1, float interval = 0.1f, bool wait = true)
        {
            new AltUnityClickCoordinates(socketSettings, coordinates, count, interval, wait).Execute();
        }
        public void Tilt(AltUnityVector3 acceleration, float duration = 0)
        {
            new AltUnityTilt(socketSettings, acceleration, duration).Execute();
        }
        public void TiltAndWait(AltUnityVector3 acceleration, float duration = 0)
        {
            new AltUnityTiltAndWait(socketSettings, acceleration, duration).Execute();
        }

        public List<AltUnityObject> GetAllElements(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityGetAllElements(socketSettings, cameraBy, cameraPath, enabled).Execute();
        }
        public List<AltUnityObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityGetAllElementsLight(socketSettings, cameraBy, cameraPath, enabled).Execute();
        }

        public string WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
        {
            return new AltUnityWaitForCurrentSceneToBe(socketSettings, sceneName, timeout, interval).Execute();
        }

        public AltUnityObject WaitForObject(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObject(socketSettings, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        public void WaitForObjectNotBePresent(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            new AltUnityWaitForObjectNotBePresent(socketSettings, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        public AltUnityObject WaitForObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObjectWhichContains(socketSettings, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        [System.ObsoleteAttribute("Use instead WaitForObject")]
        public AltUnityObject WaitForObjectWithText(By by, string value, string text, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObjectWithText(socketSettings, by, value, text, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        public List<string> GetAllScenes()
        {
            return new AltUnityGetAllScenes(socketSettings).Execute();
        }
        public List<AltUnityObject> GetAllCameras()
        {
            return new AltUnityGetAllCameras(socketSettings).Execute();
        }

        public List<AltUnityObject> GetAllActiveCameras()
        {
            return new AltUnityGetAllActiveCameras(socketSettings).Execute();
        }
        public AltUnityTextureInformation GetScreenshot(AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            return new AltUnityGetScreenshot(socketSettings, size, screenShotQuality).Execute();
        }
        public AltUnityTextureInformation GetScreenshot(int id, AltUnityColor color, float width, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            return new AltUnityGetScreenshot(socketSettings, id, color, width, size, screenShotQuality).Execute();
        }
        public AltUnityTextureInformation GetScreenshot(AltUnityVector2 coordinates, AltUnityColor color, float width, out AltUnityObject selectedObject, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
        {
            return new AltUnityGetScreenshot(socketSettings, coordinates, color, width, size, screenShotQuality).Execute(out selectedObject);
        }
        public void GetPNGScreenshot(string path)
        {
            new AltUnityGetPNGScreenshot(socketSettings, path).Execute();
        }
        public List<AltUnityObjectLight> GetAllLoadedScenesAndObjects(bool enabled = true)
        {
            return new AltUnityGetAllLoadedScenesAndObjects(socketSettings, enabled).Execute();
        }

        public void SetServerLogging(AltUnityLogger logger, AltUnityLogLevel logLevel)
        {
            new AltUnitySetServerLogging(socketSettings, logger, logLevel).Execute();
        }
        public int BeginTouch(AltUnityVector2 screenPosition)
        {
            return new AltUnityBeginTouch(socketSettings, screenPosition).Execute();
        }
        public void MoveTouch(int fingerId, AltUnityVector2 screenPosition)
        {
            new AltUnityMoveTouch(socketSettings, fingerId, screenPosition).Execute();
        }
        public void EndTouch(int fingerId)
        {
            new AltUnityEndTouch(socketSettings, fingerId).Execute();
        }

    }
}