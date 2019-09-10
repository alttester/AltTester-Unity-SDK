
using System;
using System.Net.Sockets;

public enum PLayerPrefKeyType { Int = 1, String, Float }
public struct SocketSettings
{
    public System.Net.Sockets.TcpClient socket;
    public string requestSeparator;
    public string requestEnding;

    public SocketSettings(TcpClient socket, string requestSeparator, string requestEnding)
    {
        this.socket = socket;
        this.requestSeparator = requestSeparator;
        this.requestEnding = requestEnding;
    }
}
public class AltUnityDriver
{
    public System.Net.Sockets.TcpClient Socket;
    public static SocketSettings socketSettings;
    private static string tcp_ip = "127.0.0.1";
    private static int tcp_port = 13000;
    public static string requestSeparatorString;
    public static string requestEndingString;
    public AltUnityDriver(string tcp_ip = "127.0.0.1", int tcp_port = 13000, string requestSeparator = ";", string requestEnding = "&")
    {
        Socket = new System.Net.Sockets.TcpClient();
        Socket.Connect(tcp_ip, tcp_port);
        AltUnityObject.altUnityDriver = this;
        socketSettings = new SocketSettings(Socket, requestSeparator, requestEnding);
    }

    public void Stop()
    {
       new StopCommandDriver(socketSettings).Execute();
    }
    public void LoadScene(string scene)
    {
        new LoadSceneDriver(socketSettings, scene).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> FindObjects(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new FindObjectsDriver(socketSettings, by, value, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> FindObjectsWhichContain(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new FindObjectsWhichContainDriver(socketSettings, by, value, cameraName, enabled).Execute();
    }
    public AltUnityObject FindObject(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new FindObjectDriver(socketSettings, by, value, cameraName, enabled).Execute();
    }
    public AltUnityObject FindObjectWhichContains(By by, string value, string cameraName = "", bool enabled = true)
    {
        return new FindObjectWhichContainsDriver(socketSettings, by, value, cameraName, enabled).Execute();
    }
    public void SetTimeScale(float timeScale)
    {
        new SetTimeScaleDriver(socketSettings, timeScale).Execute();
    }
    public float GetTimeScale()
    {
        return new GetTimeScaleDriver(socketSettings).Execute();
    }
    public string CallStaticMethods(string typeName, string methodName,
        string parameters, string typeOfParameters = "", string assemblyName = "")
    {
        return new CallStaticMethodsDriver(socketSettings, typeName, methodName, parameters, typeName, assemblyName).Execute();
    }
    public void DeletePlayerPref()
    {
        new DeletePlayerPrefDriver(socketSettings).Execute();
    }
    public void DeleteKeyPlayerPref(string keyName)
    {
        new DeleteKeyPlayerPrefDriver(socketSettings, keyName).Execute();
    }
    public void SetKeyPlayerPref(string keyName, int valueName)
    {
        new SetKeyPLayerPrefDriver(socketSettings, keyName, valueName).Execute();
    }
    public void SetKeyPlayerPref(string keyName, float valueName)
    {
        new SetKeyPLayerPrefDriver(socketSettings, keyName, valueName).Execute();
    }
    public void SetKeyPlayerPref(string keyName, string valueName)
    {
        new SetKeyPLayerPrefDriver(socketSettings, keyName, valueName).Execute();
    }
    public int GetIntKeyPlayerPref(string keyName)
    {
        return new GetIntKeyPLayerPrefDriver(socketSettings, keyName).Execute();
    }
    public float GetFloatKeyPlayerPref(string keyName)
    {
        return new GetFloatKeyPlayerPrefDriver(socketSettings, keyName).Execute();
    }
    public string GetStringKeyPlayerPref(string keyName)
    {
        return new GetStringKeyPlayerPrefDriver(socketSettings, keyName).Execute();
    }
    public string GetCurrentScene()
    {
        return new GetCurrentSceneDriver(socketSettings).Execute();
    }
    public void Swipe(UnityEngine.Vector2 start, UnityEngine.Vector2 end, float duration)
    {
        new SwipeDriver(socketSettings, start, end, duration).Execute();
    }
    public void SwipeAndWait(UnityEngine.Vector2 start, UnityEngine.Vector2 end, float duration)
    {
        new SwipeAndWaitDriver(socketSettings, start, end, duration).Execute();
    }
    public void HoldButton(UnityEngine.Vector2 position, float duration)
    {
        Swipe(position, position, duration);
    }
    public void HoldButtonAndWait(UnityEngine.Vector2 position, float duration)
    {
        SwipeAndWait(position, position, duration);
    }
    public void PressKey(UnityEngine.KeyCode keyCode, float power = 1, float duration = 1)
    {
        new PressKeyDriver(socketSettings, keyCode, power, duration).Execute();
    }
    public void PressKeyAndWait(UnityEngine.KeyCode keyCode, float power = 1, float duration = 1)
    {
        new PressKeyAndWaitDriver(socketSettings, keyCode, power, duration).Execute();
    }
    public void MoveMouse(UnityEngine.Vector2 location, float duration = 0)
    {
        new MoveMouseDriver(socketSettings, location, duration).Execute();
    }

    public void MoveMouseAndWait(UnityEngine.Vector2 location, float duration = 0)
    {
        new MoveMouseAndWaitDriver(socketSettings, location, duration).Execute();
    }

    public void ScrollMouse(float speed, float duration = 0)
    {
        new ScrollMouseDriver(socketSettings, speed, duration).Execute();
    }

    public void ScrollMouseAndWait(float speed, float duration = 0)
    {
        new ScrollMouseAndWaitDriver(socketSettings, speed, duration).Execute();
    }
    public AltUnityObject TapScreen(float x, float y)
    {
        return new TapScreenDriver(socketSettings, x, y).Execute();
    }
    public void Tilt(UnityEngine.Vector3 acceleration)
    {
        new TiltDriver(socketSettings, acceleration).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObjectByNameContains")]
    public AltUnityObject FindElementWhereNameContains(string name, string cameraName = "", bool enabled = true)
    {
        return new FindElementWhereNameContainsDriver(socketSettings, name, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<AltUnityObject> GetAllElements(string cameraName = "", bool enabled = true)
    {
        return new GetAllElementsDriver(socketSettings, cameraName, enabled).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObjectByName")]
    public AltUnityObject FindElement(string name, string cameraName = "", bool enabled = true)
    {
        return new FindElementDriver(socketSettings, name, cameraName, enabled).Execute();
    }
    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public System.Collections.Generic.List<AltUnityObject> FindElements(string name, string cameraName = "", bool enabled = true)
    {
        return new FindElementsDriver(socketSettings, name, cameraName, enabled).Execute();
    }

    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public System.Collections.Generic.List<AltUnityObject> FindElementsWhereNameContains(string name, string cameraName = "", bool enabled = true)
    {
        return new FindElementsWhereNameContainsDriver(socketSettings, name, cameraName, enabled).Execute();
    }
    public string WaitForCurrentSceneToBe(string sceneName, double timeout = 10, double interval = 1)
    {
        return new WaitForCurrentSceneToBeDriver(socketSettings, sceneName, timeout, interval).Execute();
    }

    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public AltUnityObject WaitForElementWhereNameContains(string name, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new WaitForElementWhereNameContainsDriver(socketSettings, name, cameraName, enabled, timeout, interval).Execute();
    }
    public AltUnityObject WaitForObject(By by, string value, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new WaitForObjectDriver(socketSettings, by, value, cameraName, enabled, timeout, interval).Execute();
    }

    public void WaitForObjectNotBePresent(By by, string value, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        new WaitForObjectNotBePresentDriver(socketSettings, by, value, cameraName, enabled, timeout, interval).Execute();
    }


    [System.ObsoleteAttribute("Use instead WaitForObjectNotBePresent")]
    public void WaitForElementToNotBePresent(string name, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        new WaitForElementToNotBePresentDriver(socketSettings, name, cameraName, enabled, timeout, interval).Execute();
    }
    [System.ObsoleteAttribute("Use instead WaitForObject")]
    public AltUnityObject WaitForElement(string name, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new WaitForElementDriver(socketSettings, name, cameraName, enabled, timeout, interval).Execute();
    }

    [System.ObsoleteAttribute("Use instead WaitForObjectWithText")]
    public AltUnityObject WaitForElementWithText(string name, string text, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new WaitForElementWithText(socketSettings, name, text, cameraName, enabled, timeout, interval).Execute();
    }
    public AltUnityObject WaitForObjectWithText(By by, string value, string text, string cameraName = "", bool enabled = true, double timeout = 20, double interval = 0.5)
    {
        return new WaitForObjectWithTextDriver(socketSettings, by, value, text, cameraName, enabled, timeout, interval).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObjectByComponent")]
    public AltUnityObject FindElementByComponent(string componentName, string assemblyName = "", string cameraName = "", bool enabled = true)
    {
        return new FindElementByComponentDriver(socketSettings, componentName, assemblyName, cameraName, enabled).Execute();
    }
    [System.ObsoleteAttribute("Use instead FindObjectsByComponent")]
    public System.Collections.Generic.List<AltUnityObject> FindElementsByComponent(string componentName, string assemblyName = "", string cameraName = "", bool enabled = true)
    {
        return new FindElementsByComponentDriver(socketSettings, componentName, assemblyName, cameraName, enabled).Execute();
    }
    public System.Collections.Generic.List<string> GetAllScenes()
    {
        return new GetAllScenesDriver(socketSettings).Execute();
    }
    public System.Collections.Generic.List<string> GetAllCameras()
    {
        return new GetAllCamerasDriver(socketSettings).Execute();
    }
    public TextureInformation GetScreenshot(UnityEngine.Vector2 size = default(UnityEngine.Vector2))
    {
        return new GetScreenshotDriver(socketSettings, size).Execute();
    }
    public TextureInformation GetScreenshot(int id, UnityEngine.Color color, float width, UnityEngine.Vector2 size = default(UnityEngine.Vector2))
    {
        return new GetScreenshotDriver(socketSettings, id, color, width, size).Execute();
    }
    public TextureInformation GetScreenshot(UnityEngine.Vector2 coordinates, UnityEngine.Color color, float width, UnityEngine.Vector2 size = default(UnityEngine.Vector2))
    {
        return new GetScreenshotDriver(socketSettings, coordinates, color, width, size).Execute();

    }
}