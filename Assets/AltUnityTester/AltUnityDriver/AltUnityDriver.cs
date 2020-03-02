using Assets.AltUnityTester.AltUnityDriver.Commands.InputActions;
using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public enum PLayerPrefKeyType { Int = 1, String, Float }
public struct SocketSettings
{
    public System.Net.Sockets.TcpClient socket;
    public string requestSeparator;
    public string requestEnding;
    public bool logFlag;

    public SocketSettings(System.Net.Sockets.TcpClient socket, string requestSeparator, string requestEnding,bool logFlag)
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
    public static readonly string VERSION="1.5.3";
    private static string tcp_ip = "127.0.0.1";
    private static int tcp_port = 13000;
    public static string requestSeparatorString;
    public static string requestEndingString;

    public AltUnityDriver(string tcp_ip = "127.0.0.1", int tcp_port = 13000, string requestSeparator = ";", string requestEnding = "&", bool logFlag = false)
    {
        
        int timeout=60;
        while(timeout>0){
            try
            {
                Socket = new System.Net.Sockets.TcpClient();
                Socket.Connect(tcp_ip, tcp_port);
                Socket.SendTimeout = 5000;
                Socket.ReceiveTimeout = 5000;

                socketSettings = new SocketSettings(Socket, requestSeparator, requestEnding, logFlag);
                EnableLogging();
                CheckServerVersion();
                break;
            }
            catch(System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                timeout -= 5;
                System.Threading.Thread.Sleep(5000);
            }
            
        }
        
    }
    private void CheckServerVersion()
    {
        new AltUnityCheckServerVersion(socketSettings).Execute();
    }
    private void EnableLogging(){
        new AltUnityEnableLogging(socketSettings).Execute();
    }

    public void Stop()
    {
       new AltUnityStopCommand(socketSettings).Execute();
    }
    public void LoadScene(string scene)
    {
        new AltUnityLoadScene(socketSettings, scene).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> FindObjects(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindObjects (socketSettings, by, value, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> FindObjectsWhichContain(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindObjectsWhichContain(socketSettings, by, value, cameraName, enabled).Execute();
    }
    public AltUnityObject FindObject(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindObject(socketSettings, by, value, cameraName, enabled).Execute();
    }
    public AltUnityObject FindObjectWhichContains(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindObjectWhichContains(socketSettings, by, value, cameraName, enabled).Execute();
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
        return new AltUnityCallStaticMethods(socketSettings, typeName, methodName, parameters, typeName, assemblyName).Execute();
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
        new AltUnityAltUnitySwipe(socketSettings, start, end, duration).Execute();
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
    public void Tilt(AltUnityVector3 acceleration)
    {
        new AltUnityTilt(socketSettings, acceleration).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObjectWhichContains")]
    public AltUnityObject FindElementWhereNameContains(string name, string cameraName = "", bool enabled = true)
    {
        return new AltUnityFindElementWhereNameContains(socketSettings, name, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> GetAllElements(string cameraName = "", bool enabled = true)
    {
        return new AltUnityGetAllElements(socketSettings, cameraName, enabled).Execute();
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
    public AltUnityObject WaitForObject(By by, string value, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObject(socketSettings, by, value, cameraName, enabled, timeout, interval).Execute();
    }

    public void WaitForObjectNotBePresent(By by, string value, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        new AltUnityWaitForObjectNotBePresent(socketSettings, by, value, cameraName, enabled, timeout, interval).Execute();
    }

    public AltUnityObject WaitForObjectWhichContains(By by, string value, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5){
        return new AltUnityWaitForObjectWhichContains (socketSettings,by,value,cameraName,enabled,timeout,interval).Execute();
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
    public AltUnityObject WaitForObjectWithText(By by, string value, string text, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new AltUnityWaitForObjectWithText(socketSettings, by, value, text, cameraName, enabled, timeout, interval).Execute();
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
    public System.Collections.Generic.List<string> GetAllCameras()
    {
        return new AltUnityGetAllCameras(socketSettings).Execute();
    }
    public AltUnityTextureInformation GetScreenshot(AltUnityVector2 size = default(AltUnityVector2))
    {
        return new AltUnityGetScreenshot(socketSettings, size).Execute();
    }
    public AltUnityTextureInformation GetScreenshot(int id, Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityColor color, float width, AltUnityVector2 size = default(AltUnityVector2))
    {
        return new AltUnityGetScreenshot(socketSettings, id, color, width, size).Execute();
    }
    public AltUnityTextureInformation GetScreenshot(AltUnityVector2 coordinates, Assets.AltUnityTester.AltUnityDriver.UnityStruct.AltUnityColor color, float width, AltUnityVector2 size = default(AltUnityVector2))
    {
        return new AltUnityGetScreenshot(socketSettings, coordinates, color, width, size).Execute();

    }
    public void GetPNGScreenshot(string path)
    {
        new AltUnityGetPNGScreenshot (socketSettings,path).Execute();
    }
}