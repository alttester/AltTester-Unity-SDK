using Altom.AltUnityDriver.AltSocket;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityDriver
{
    public enum By
    {
        TAG, LAYER, NAME, COMPONENT, PATH, ID
    }

    public class AltUnityDriver
    {
        public System.Net.Sockets.TcpClient Socket;
        public SocketSettings socketSettings;
        public static readonly string VERSION = "1.6.2";
        public static string requestSeparatorString;
        public static string requestEndingString;

        /// <summary>
        /// Initiates AltUnity Driver and begins connection to AltUnity Server
        /// </summary>
        /// <param name="tcp_ip">The ip or hostname  AltUnity Server is listening on.</param>
        /// <param name="tcp_port">The port AltUnity Server is listening on.</param>
        /// <param name="requestSeparator">The separator of command parameters. Must match requestSeparatorString in AltUnity Server </param>
        /// <param name="requestEnding">The ending of the command. Must match requestEnding in AltUnity Server </param>
        /// <param name="logFlag">If true it enables extended logs in AltUnity Server and writes commands response to log file.</param>
        public AltUnityDriver(string tcp_ip = "127.0.0.1", int tcp_port = 13000, string requestSeparator = ";", string requestEnding = "&", bool logFlag = false)
        {
            int timeout = 60;
            int retryPeriod = 5;
            while (timeout > 0)
            {
                try
                {
                    Socket = new System.Net.Sockets.TcpClient();
                    Socket.Connect(tcp_ip, tcp_port);
                    Socket.SendTimeout = 5000;
                    Socket.ReceiveTimeout = 5000;
                    var altSocket = new Socket(Socket.Client);
                    socketSettings = new SocketSettings(altSocket, requestSeparator, requestEnding, logFlag);
                    checkServerVersion();
                    break;
                }
                catch (System.Exception ex)
                {
                    if (Socket != null)
                        Stop();

                    string errorMessage = "Trying to reach AltUnity Server at port" + tcp_port + ",retrying in " + retryPeriod + " (timing out in " + timeout + " secs)...";
                    System.Console.WriteLine(errorMessage);
#if UNITY_EDITOR
                    UnityEngine.Debug.Log(errorMessage);
#endif

                    timeout -= retryPeriod;
                    if (timeout <= 0)
                    {
                        throw new System.Exception("Could not create connection to " + tcp_ip + ":" + tcp_port, ex);
                    }
                    System.Threading.Thread.Sleep(retryPeriod * 1000);
                }
            }
            try
            {
                EnableLogging();
            }
            catch (AltUnityRecvallMessageFormatException)
            {
                System.Console.WriteLine("Cannot set logging flag because of version incompatibility.");
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Cannot set logging flag because of version incompatibility.");
#endif
            }
        }
        private void splitVersion(string version, out string major, out string minor)
        {
            var parts = version.Split(new[] { "." }, System.StringSplitOptions.None);
            major = parts[0];
            minor = parts.Length > 1 ? parts[1] : string.Empty;
        }
        private void checkServerVersion()
        {
            string serverVersion;
            try
            {
                serverVersion = new AltUnityGetServerVersion(socketSettings).Execute();
            }
            catch (UnknownErrorException)
            {
                serverVersion = "<=1.5.3";
            }
            catch (AltUnityRecvallMessageFormatException)
            {
                serverVersion = "<=1.5.7";
            }
            string majorServer, minorServer,
            majorDriver, minorDriver;

            splitVersion(serverVersion, out majorServer, out minorServer);
            splitVersion(AltUnityDriver.VERSION, out majorDriver, out minorDriver);

            if (majorServer != majorDriver || minorServer != minorDriver)
            {
                string message = "Version mismatch. AltUnity Driver version is " + AltUnityDriver.VERSION + ". AltUnity Server version is " + serverVersion + ".";

#if UNITY_EDITOR
                UnityEngine.Debug.LogWarning(message);
#endif
                System.Console.WriteLine(message);
            }
        }
        private void EnableLogging()
        {
            new AltUnityEnableLogging(socketSettings).Execute();
        }

        public void Stop()
        {
            new AltUnityStopCommand(socketSettings).Execute();
            Socket.Close();
        }
        public void LoadScene(string scene, bool loadSingle = true)
        {
            new AltUnityLoadScene(socketSettings, scene, loadSingle).Execute();
        }
        public System.Collections.Generic.List<string> GetAllLoadedScenes()
        {
            return new AltUnityGetAllLoadedScenes(socketSettings).Execute();
        }

        public System.Collections.Generic.List<AltUnityObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityFindObjects(socketSettings, by, value, cameraBy, cameraPath, enabled).Execute();
        }

        public System.Collections.Generic.List<AltUnityObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
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
        [System.ObsoleteAttribute("Use instead CallStaticMethod")]
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
        public void PressKey(AltUnityKeyCode keyCode, float power = 1, float duration = 1)
        {
            new AltUnityPressKey(socketSettings, keyCode, power, duration).Execute();
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
        public AltUnityObject TapScreen(float x, float y)
        {
            return new AltUnityTapScreen(socketSettings, x, y).Execute();
        }
        public void TapCustom(float x, float y, int count, float interval = 0.1f)
        {
            new AltUnityTapCustom(socketSettings, x, y, count, interval).Execute();
        }
        public void Tilt(AltUnityVector3 acceleration, float duration = 0)
        {
            new AltUnityTilt(socketSettings, acceleration, duration).Execute();
        }
        public void TiltAndWait(AltUnityVector3 acceleration, float duration = 0)
        {
            new AltUnityTiltAndWait(socketSettings, acceleration, duration).Execute();
        }

        public System.Collections.Generic.List<AltUnityObject> GetAllElements(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
        {
            return new AltUnityGetAllElements(socketSettings, cameraBy, cameraPath, enabled).Execute();
        }
        public System.Collections.Generic.List<AltUnityObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
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

        public AltUnityObject WaitForObjectWithText(By by, string value, string text, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
        {
            return new AltUnityWaitForObjectWithText(socketSettings, by, value, text, cameraBy, cameraPath, enabled, timeout, interval).Execute();
        }

        public System.Collections.Generic.List<string> GetAllScenes()
        {
            return new AltUnityGetAllScenes(socketSettings).Execute();
        }
        public System.Collections.Generic.List<AltUnityObject> GetAllCameras()
        {
            return new AltUnityGetAllCameras(socketSettings).Execute();
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
    }
}