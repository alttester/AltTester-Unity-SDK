using System.Linq;
using Assets.AltUnityTester.AltUnityDriver.Commands.InputActions;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public enum PLayerPrefKeyType { Int = 1, String, Float }
public struct SocketSettings
{
    public System.Net.Sockets.TcpClient socket;
    public string requestSeparator;
    public string requestEnding;
    public bool logFlag;

    public SocketSettings(System.Net.Sockets.TcpClient socket, string requestSeparator, string requestEnding, bool logFlag)
    {
        this.socket = socket;
        this.requestSeparator = requestSeparator;
        this.requestEnding = requestEnding;
        this.logFlag = logFlag;
    }
}
public class AltUnityDriver
{
    public System.Net.Sockets.TcpClient Socket;
    public SocketSettings socketSettings;
    public static readonly string VERSION = "1.6.0-alpha";
    public static string requestSeparatorString;
    public static string requestEndingString;

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

                socketSettings = new SocketSettings(Socket, requestSeparator, requestEnding, logFlag);
                checkServerVersion();
                EnableLogging();
                break;
            }
            catch (System.Exception)
            {
                if (Socket != null)
                    Stop();

                string errorMessage = "Trying to reach AltUnity Server at port" + tcp_port + ",retrying in " + retryPeriod + " (timing out in " + timeout + " secs)...";
                System.Console.WriteLine(errorMessage);
#if UNITY_EDITOR
                UnityEngine.Debug.Log(errorMessage);
#endif

                timeout -= retryPeriod;
                System.Threading.Thread.Sleep(retryPeriod * 1000);
            }
            if (timeout <= 0)
            {
                throw new System.Exception("Could not create connection to " + tcp_ip + ":" + tcp_port);
            }
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
        catch (Assets.AltUnityTester.AltUnityDriver.UnknownErrorException)
        {
            serverVersion = "<=1.5.3";
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
    [System.Obsolete()]
    public System.Collections.Generic.List<AltUnityObject> FindObjects(By by, string value, string cameraName, bool enabled = true)
    {
        return new AltUnityFindObjects(socketSettings, by, value, By.NAME, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> FindObjects(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
    {
        return new AltUnityFindObjects(socketSettings, by, value, cameraBy, cameraPath, enabled).Execute();
    }
    [System.Obsolete()]
    public System.Collections.Generic.List<AltUnityObject> FindObjectsWhichContain(By by, string value, string cameraName, bool enabled = true)
    {
        return new AltUnityFindObjectsWhichContain(socketSettings, by, value, By.NAME, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> FindObjectsWhichContain(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
    {
        return new AltUnityFindObjectsWhichContain(socketSettings, by, value, cameraBy, cameraPath, enabled).Execute();
    }
    [System.Obsolete()]
    public AltUnityObject FindObject(By by, string value, string cameraName, bool enabled = true)
    {
        return new AltUnityFindObject(socketSettings, by, value, By.NAME, cameraName, enabled).Execute();
    }
    public AltUnityObject FindObject(By by, string value, By cameraBy = By.NAME, string cameraValue = "", bool enabled = true)
    {
        return new AltUnityFindObject(socketSettings, by, value, cameraBy, cameraValue, enabled).Execute();
    }
    [System.Obsolete()]
    public AltUnityObject FindObjectWhichContains(By by, string value, string cameraName, bool enabled = true)
    {
        return new AltUnityFindObjectWhichContains(socketSettings, by, value, By.NAME, cameraName, enabled).Execute();
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
    public string CallStaticMethods(string typeName, string methodName,
        string parameters, string typeOfParameters = "", string assemblyName = "")
    {
        return new AltUnityCallStaticMethods(socketSettings, typeName, methodName, parameters, typeOfParameters, assemblyName).Execute();
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
    public void PressKey(Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityKeyCode keyCode, float power = 1, float duration = 1)
    {
        new AltUnityPressKey(socketSettings, keyCode, power, duration).Execute();
    }
    public void PressKeyAndWait(Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityKeyCode keyCode, float power = 1, float duration = 1)
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
    [System.ObsoleteAttribute("Use instead FindObjectWhichContains")]
    public AltUnityObject FindElementWhereNameContains(string name, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElementWhereNameContains(socketSettings, name, cameraName, enabled).Execute();
    }
    [System.Obsolete()]
    public System.Collections.Generic.List<AltUnityObject> GetAllElements(string cameraName, bool enabled = true)
    {
        return new AltUnityGetAllElements(socketSettings, By.NAME, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> GetAllElements(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
    {
        return new AltUnityGetAllElements(socketSettings, cameraBy, cameraPath, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObjectLight> GetAllElementsLight(By cameraBy = By.NAME, string cameraPath = "", bool enabled = true)
    {
        return new AltUnityGetAllElementsLight(socketSettings, cameraBy, cameraPath, enabled).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObject")]
    public AltUnityObject FindElement(string name, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElement(socketSettings, name, cameraName, enabled).Execute();
    }
    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public System.Collections.Generic.List<AltUnityObject> FindElements(string name, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElements(socketSettings, name, cameraName, enabled).Execute();
    }

    [System.ObsoleteAttribute("Use instead WaitForObjectWhichContains")]
    public System.Collections.Generic.List<AltUnityObject> FindElementsWhereNameContains(string name, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElementsWhereNameContains(socketSettings, name, cameraName, enabled).Execute();
    }
    public string WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
    {
        return new AltUnityWaitForCurrentSceneToBe(socketSettings, sceneName, timeout, interval).Execute();
    }

    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public AltUnityObject WaitForElementWhereNameContains(string name, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForElementWhereNameContains(socketSettings, name, cameraName, enabled, timeout, interval).Execute();
    }
    [System.Obsolete()]
    public AltUnityObject WaitForObject(By by, string value, string cameraName, bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObject(socketSettings, by, value, By.NAME, cameraName, enabled, timeout, interval).Execute();
    }
    public AltUnityObject WaitForObject(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObject(socketSettings, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
    }
    [System.Obsolete()]
    public void WaitForObjectNotBePresent(By by, string value, string cameraName, bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        new AltUnityWaitForObjectNotBePresent(socketSettings, by, value, By.NAME, cameraName, enabled, timeout, interval).Execute();
    }
    public void WaitForObjectNotBePresent(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        new AltUnityWaitForObjectNotBePresent(socketSettings, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
    }
    [System.Obsolete("Use instead WaitForObjectWhichContains")]
    public AltUnityObject WaitForObjectWhichContains(By by, string value, string cameraName, bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObjectWhichContains(socketSettings, by, value, By.NAME, cameraName, enabled, timeout, interval).Execute();
    }
    public AltUnityObject WaitForObjectWhichContains(By by, string value, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObjectWhichContains(socketSettings, by, value, cameraBy, cameraPath, enabled, timeout, interval).Execute();
    }


    [System.ObsoleteAttribute("Use instead WaitForObjectNotBePresent")]
    public void WaitForElementToNotBePresent(string name, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        new AltUnityWaitForElementToNotBePresent(socketSettings, name, cameraName, enabled, timeout, interval).Execute();
    }
    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public AltUnityObject WaitForElement(string name, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForElement(socketSettings, name, cameraName, enabled, timeout, interval).Execute();
    }

    [System.ObsoleteAttribute("Use instead WaitForObjectWithText")]
    public AltUnityObject WaitForElementWithText(string name, string text, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForElementWithText(socketSettings, name, text, cameraName, enabled, timeout, interval).Execute();
    }
    [System.Obsolete()]
    public AltUnityObject WaitForObjectWithText(By by, string value, string text, string cameraName, bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObjectWithText(socketSettings, by, value, text, By.NAME, cameraName, enabled, timeout, interval).Execute();
    }
    public AltUnityObject WaitForObjectWithText(By by, string value, string text, By cameraBy = By.NAME, string cameraPath = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObjectWithText(socketSettings, by, value, text, cameraBy, cameraPath, enabled, timeout, interval).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObject")]
    public AltUnityObject FindElementByComponent(string componentName, string assemblyName = "", string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElementByComponent(socketSettings, componentName, assemblyName, cameraName, enabled).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObjects")]
    public System.Collections.Generic.List<AltUnityObject> FindElementsByComponent(string componentName, string assemblyName = "", string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElementsByComponent(socketSettings, componentName, assemblyName, cameraName, enabled).Execute();
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
    public AltUnityTextureInformation GetScreenshot(int id, Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityColor color, float width, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
    {
        return new AltUnityGetScreenshot(socketSettings, id, color, width, size, screenShotQuality).Execute();
    }
    public AltUnityTextureInformation GetScreenshot(AltUnityVector2 coordinates, Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityColor color, float width, out AltUnityObject selectedObject, AltUnityVector2 size = default(AltUnityVector2), int screenShotQuality = 100)
    {
        return new AltUnityGetScreenshot(socketSettings, coordinates, color, width, size, screenShotQuality).Execute(out selectedObject);

    }
    public void GetPNGScreenshot(string path)
    {
        new AltUnityGetPNGScreenshot(socketSettings, path).Execute();
    }

}