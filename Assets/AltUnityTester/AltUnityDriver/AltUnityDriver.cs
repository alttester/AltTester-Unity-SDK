using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Assets.AltUnityTester.AltUnityDriver;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Experimental.Rendering;


public enum PLayerPrefKeyType { Int = 1, String, Float }

public class AltUnityDriver
{
    public TcpClient Socket;
    private static String tcp_ip = "127.0.0.1";
    private static int tcp_port = 13000;
    private static int BUFFER_SIZE = 1024;
    public static string requestSeparatorString;
    public static string requestEndingString;
    public AltUnityDriver(string tcp_ip = "127.0.0.1", int tcp_port = 13000, string requestSeparator = ";", string requestEnding = "&")
    {

        Socket = new TcpClient();
        Socket.Connect(tcp_ip, tcp_port);
        AltUnityObject.altUnityDriver = this;
        requestSeparatorString = requestSeparator;
        requestEndingString = requestEnding;

    }

    public void Stop()
    {
        Socket.Client.Send(toBytes(CreateCommand("closeConnection")));
        Thread.Sleep(1000);
        Socket.Close();




    }
    public string CreateCommand(params string[] arguments)
    {
        string command = "";
        foreach(var argument in arguments)
        {
            command += argument + requestSeparatorString;
        }
        command += requestEndingString;
        return command;
    }
    public string Recvall()
    {

        String data = "";
        String previousPart = "";
        while (true)
        {
            var bytesReceived = new byte[BUFFER_SIZE];
            Socket.Client.Receive(bytesReceived);
            String part = fromBytes(bytesReceived);
            String partToSeeAltEnd = previousPart + part;
            data += part;
            if (partToSeeAltEnd.Contains("::altend"))
                break;
            previousPart = part;
        }

        try
        {
            string[] start = new string[] { "altstart::" };
            string[] end = new string[] { "::altend" };
            data = data.Split(start, StringSplitOptions.None)[1].Split(end, StringSplitOptions.None)[0];
        }
        catch (Exception)
        {
            Debug.Log("Data received from socket doesn't have correct start and end control strings");
        }

        return data;
    }

    private byte[] toBytes(String text)
    {
        return Encoding.ASCII.GetBytes(text);
    }

    private String fromBytes(byte[] text)
    {
        return Encoding.ASCII.GetString(text);
    }

    public void LoadScene(string scene)
    {
        Socket.Client.Send(toBytes(CreateCommand("loadScene",scene)));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }

    public void SetTimeScale(float timeScale)
    {
        Socket.Client.Send(toBytes(CreateCommand("setTimeScale", JsonConvert.SerializeObject(timeScale))));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }

    public float GetTimeScale()
    {
        Socket.Client.Send(toBytes(CreateCommand("getTimeScale")));
        var data = Recvall();
        if (!data.Contains("error"))
            return JsonConvert.DeserializeObject<float>(data);
        HandleErrors(data);
        return -1f;
    }

    public string CallStaticMethods(String typeName, String methodName,
        String parameters, String typeOfParameters = "", String assemblyName = "")
    {
        String actionInfo =
            JsonConvert.SerializeObject(new AltUnityObjectAction(typeName, methodName, parameters, typeOfParameters, assemblyName));
        Socket.Client.Send(toBytes(CreateCommand("callComponentMethodForObject","",actionInfo)));
        var data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;
    }
    public void DeletePlayerPref()
    {
        Socket.Client.Send(toBytes(CreateCommand("deletePlayerPref")));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public void DeleteKeyPlayerPref(string keyName)
    {
        Socket.Client.Send(toBytes(CreateCommand("deleteKeyPlayerPref" , keyName )));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public void SetKeyPlayerPref(string keyName, int valueName)
    {
        Socket.Client.Send(toBytes(CreateCommand("setKeyPlayerPref", keyName , valueName.ToString() , PLayerPrefKeyType.Int.ToString() )));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;

        HandleErrors(data);


    }
    public void SetKeyPlayerPref(string keyName, float valueName)
    {
        Socket.Client.Send(toBytes(CreateCommand("setKeyPlayerPref", keyName , valueName.ToString(),PLayerPrefKeyType.Float.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public void SetKeyPlayerPref(string keyName, string valueName)
    {
        Socket.Client.Send(toBytes(CreateCommand("setKeyPlayerPref", keyName , valueName.ToString(), PLayerPrefKeyType.String.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public int GetIntKeyPlayerPref(string keyname)
    {
        Socket.Client.Send(toBytes(CreateCommand("getKeyPlayerPref", keyname, PLayerPrefKeyType.Int.ToString())));
        var data = Recvall();
        if (!data.Contains("error:")) return Int32.Parse(data);
        HandleErrors(data);
        return 0;

    }
    public float GetFloatKeyPlayerPref(string keyname)
    {
        Socket.Client.Send(toBytes(CreateCommand("getKeyPlayerPref" , keyname , PLayerPrefKeyType.Float.ToString())));
        var data = Recvall();
        if (!data.Contains("error:")) return Single.Parse(data);
        HandleErrors(data);
        return 0;

    }
    public string GetStringKeyPlayerPref(string keyname)
    {
        Socket.Client.Send(toBytes(CreateCommand("getKeyPlayerPref" ,keyname , PLayerPrefKeyType.String.ToString())));
        var data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;

    }

    public String GetCurrentScene()
    {

        Socket.Client.Send(toBytes(CreateCommand("getCurrentScene")));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<AltUnityObject>(data).name;
        HandleErrors(data);
        return null;
    }


    public void Swipe(Vector2 start, Vector2 end, float duration)
    {
        String vectorStartJson = JsonConvert.SerializeObject(start, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        String vectorEndJson = JsonConvert.SerializeObject(end, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes(CreateCommand("movingTouch" ,vectorStartJson,vectorEndJson,duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }

    public void SwipeAndWait(Vector2 start, Vector2 end, float duration)
    {
        Swipe(start, end, duration);
        Thread.Sleep((int)duration * 1000);
        string data;
        do
        {
            Socket.Client.Send(toBytes(CreateCommand("swipeFinished")));
            data = Recvall();
        } while (data == "No");
        if (data.Equals("Yes"))
            return;
        HandleErrors(data);
    }
    public void HoldButton(Vector2 position, float duration)
    {
        Swipe(position, position, duration);
    }

    public void HoldButtonAndWait(Vector2 position, float duration)
    {
        SwipeAndWait(position, position, duration);
    }
    public AltUnityObject TapScreen(float x, float y)
    {
        Socket.Client.Send(toBytes(CreateCommand("tapScreen", x.ToString(), y.ToString() )));
        string data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<AltUnityObject>(data);
        if (data.Contains("error:notFound")) return null;
        HandleErrors(data);
        return null;
    }
    

    public void Tilt(Vector3 acceleration)
    {
        String accelerationString = JsonConvert.SerializeObject(acceleration, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes(CreateCommand("tilt",accelerationString)));
        string data = Recvall();
        if (data.Equals("OK")) return;
        HandleErrors(data);


    }

    public AltUnityObject FindElementWhereNameContains(String name, String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectWhereNameContains",name,cameraName,enabled.ToString() )));
        String data = Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }
        HandleErrors(data);
        return null;

    }

    public List<AltUnityObject> GetAllElements(String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findAllObjects",cameraName,enabled.ToString())));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;

    }

    public AltUnityObject FindElement(String name, String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectByName",name,cameraName,enabled.ToString())));
        String data = Recvall();
        if (!data.Contains("error:"))
        {
            return JsonConvert.DeserializeObject<AltUnityObject>(data);

        }
        HandleErrors(data);
        return null;
    }

    public List<AltUnityObject> FindElements(String name, String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectsByName", name, cameraName, enabled.ToString())));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }

    public List<AltUnityObject> FindElementsWhereNameContains(String name, String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectsWhereNameContains",name ,cameraName ,enabled.ToString() )));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }



    public String WaitForCurrentSceneToBe(String sceneName, double timeout = 10, double interval = 1)
    {
        double time = 0;
        String currentScene = "";
        while (time < timeout)
        {
            currentScene = GetCurrentScene();
            if (!currentScene.Equals(sceneName))
            {
                Debug.Log("Waiting for scene to be " + sceneName + "...");
                Thread.Sleep(Convert.ToInt32(interval * 1000));
                time += interval;
            }
            else
            {
                break;
            }
        }

        if (sceneName.Equals(currentScene))
            return currentScene;
        throw new WaitTimeOutException("Scene " + sceneName + " not loaded after " + timeout + " seconds");

    }

    public AltUnityObject WaitForElementWhereNameContains(String name, String cameraName = "", double timeout = 20, double interval = 0.5)
    {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement = FindElementWhereNameContains(name, cameraName);
                break;
            }
            catch (Exception)
            {
                Debug.Log("Waiting for element where name contains " + name + "....");
                Thread.Sleep(Convert.ToInt32(interval * 1000));
                time += interval;
            }
        }
        if (altElement != null)
            return altElement;
        throw new WaitTimeOutException("Element " + name + " still not found after " + timeout + " seconds");

    }



    public void WaitForElementToNotBePresent(String name, String cameraName = "", double timeout = 20, double interval = 0.5)
    {
        double time = 0;
        bool found = false; 
        AltUnityObject altElement = null;
        while (time <= timeout)
        {
            found = false;
            try
            {
                altElement = FindElement(name, cameraName);
                found = true;
                Thread.Sleep(Convert.ToInt32(interval * 1000));
                time += interval;
                Debug.Log("Waiting for element " + name + " to not be present");
            }
            catch (Exception)
            {
                break;
            }

        }

        if (found)
            throw new WaitTimeOutException("Element " + name + " still found after " + timeout + " seconds");
    }



    public AltUnityObject WaitForElement(String name, String cameraName = "", double timeout = 20, double interval = 0.5)
    {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement = FindElement(name, cameraName);
                break;
            }
            catch (Exception)
            {
                Thread.Sleep(Convert.ToInt32(interval * 1000));
                time += interval;
                Debug.Log("Waiting for element " + name + "...");
            }

        }

        if (altElement != null)
        {
            return altElement;
        }
        throw new WaitTimeOutException("Element " + name + " not loaded after " + timeout + " seconds");
    }


    public AltUnityObject WaitForElementWithText(String name, string text, String cameraName = "", double timeout = 20, double interval = 0.5)
    {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement = FindElement(name, cameraName);
                if (altElement.GetText().Equals(text))
                    break;
                throw new Exception("Not the wanted text");
            }
            catch (Exception)
            {
                Thread.Sleep(Convert.ToInt32(interval * 1000));
                time += interval;
                Debug.Log("Waiting for element " + name + " to have text " + text);
            }
        }
        if (altElement != null && altElement.GetText().Equals(text))
        {
            return altElement;
        }
        throw new WaitTimeOutException("Element with text: " + text + " not loaded after " + timeout + " seconds");
    }

    public AltUnityObject FindElementByComponent(String componentName, String assemblyName = "", String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectByComponent",assemblyName,componentName,cameraName,enabled.ToString() )));
        String data = Recvall();
        if (!data.Contains("error:"))
        {
            return JsonConvert.DeserializeObject<AltUnityObject>(data);
        }
        HandleErrors(data);
        return null;
    }
  
    public List<AltUnityObject> FindElementsByComponent(String componentName, String assemblyName = "", String cameraName = "", bool enabled = true)
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectsByComponent", assemblyName, componentName, cameraName, enabled.ToString())));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }

    public List<String> GetAllScenes()
    {
        Socket.Client.Send(toBytes(CreateCommand("getAllScenes")));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<String>>(data);
        HandleErrors(data);
        return null;
    }
    
    public List<String> GetAllCameras()
    {
        Socket.Client.Send(toBytes(CreateCommand("getAllCameras")));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<String>>(data);
        HandleErrors(data);
        return null;
    }

    public TextureInformation ReceiveImage() {

        var data = Recvall();
     
        string[] screenshotInfo = JsonConvert.DeserializeObject<string[]>(data);

        // Some workaround this: https://stackoverflow.com/questions/710853/base64-string-throwing-invalid-character-error
        var screenshotParts = screenshotInfo[4].Split('\0');
        screenshotInfo[4] = "";
        for (int i = 0; i < screenshotParts.Length; i++) {
            screenshotInfo[4] += screenshotParts[i];
        }

        var scaleDifference = screenshotInfo[0];    

        var length = screenshotInfo[1];
        var LongLength = JsonConvert.DeserializeObject<long>(length);
        var textureFormatString = screenshotInfo[2];
        var textureFormat = (TextureFormat)Enum.Parse(typeof(TextureFormat), textureFormatString);
        var textSizeString = screenshotInfo[3];
        var textSizeVector3 = JsonConvert.DeserializeObject<Vector3>(textSizeString);

        Byte[] imageCompressed = JsonConvert.DeserializeObject<Byte[]>(screenshotInfo[4], new JsonSerializerSettings {
            StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
        });  

        Byte[] imageDecompressed=DeCompressScreenshot(imageCompressed);
        return new TextureInformation(imageDecompressed, JsonConvert.DeserializeObject<Vector2>(scaleDifference), textSizeVector3, textureFormat);
    }

    public TextureInformation GetScreenshot(Vector2 size=default(Vector2))
    {
        var sizeSerialized = JsonConvert.SerializeObject(size);
        Socket.Client.Send(toBytes(CreateCommand("getScreenshot",sizeSerialized)));
        return ReceiveImage();
    }
    public TextureInformation GetScreenshot(int id,Color color,float width,Vector2 size = default(Vector2))
    {
        var sizeSerialized = JsonConvert.SerializeObject(size);
        var colorAndWidth = color.r + "!!" + color.g + "!!" + color.b + "!!" + color.a + "!-!" + width;
        Socket.Client.Send(toBytes(CreateCommand("hightlightObjectScreenshot",id.ToString(),colorAndWidth,sizeSerialized)));
        return ReceiveImage();
    }
    public TextureInformation GetScreenshot(Vector2 coordinates, Color color, float width, Vector2 size = default(Vector2))
    {
        var coordinatesSerialized = JsonConvert.SerializeObject(coordinates);
        var sizeSerialized = JsonConvert.SerializeObject(size);
        var colorAndWidth = color.r+"!!" + color.g + "!!" + color.b + "!!" + color.a + "!-!" + width;
        Socket.Client.Send(toBytes(CreateCommand("hightlightObjectFromCoordinatesScreenshot",coordinatesSerialized, colorAndWidth, sizeSerialized )));
        return ReceiveImage();
        
    }

    public static void HandleErrors(string data)
    {

        var typeOfException = data.Split(';')[0];
        switch (typeOfException)
        {
            case "error:notFound":
                throw new NotFoundException(data);
            case "error:propertyNotFound":
                throw new PropertyNotFoundException(data);
            case "error:methodNotFound":
                throw new MethodNotFoundException(data);
            case "error:componentNotFound":
                throw new ComponentNotFoundException(data);
            case "error:couldNotPerformOperation":
                throw new CouldNotPerformOperationException(data);
            case "error:couldNotParseJsonString":
                throw new CouldNotParseJsonStringException(data);
            case "error:incorrectNumberOfParameters":
                throw new IncorrectNumberOfParametersException(data);
            case "error:failedToParseMethodArguments":
                throw new FailedToParseArgumentsException(data);
            case "error:objectNotFound":
                throw new ObjectWasNotFoundException(data);
            case "error:propertyCannotBeSet":
                throw new PropertyNotFoundException(data);
            case "error:nullRefferenceException":
                throw new NullRefferenceException(data);
            case "error:unknownError":
                throw new UnknownErrorException(data);
            case "error:formatException":
                throw new Assets.AltUnityTester.AltUnityDriver.FormatException(data);
        }


    }

     public static byte[] DeCompressScreenshot(byte[] screenshotCompressed)
    {

        using (var memoryStreamInput = new MemoryStream(screenshotCompressed))
            using (var memoryStreamOutput = new MemoryStream())
            {
                using (var gs = new GZipStream(memoryStreamInput, CompressionMode.Decompress))
                {
                    CopyTo(gs, memoryStreamOutput);
                }

                return memoryStreamOutput.ToArray();
            }
    }
    public static T[] SubArray<T>( T[] data, int index, long length)
    {
        T[] result = new T[length];
        Array.Copy(data, index, result, 0, length);
        return result;
    }
    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    public struct TextureInformation
    {
        public byte[] imageData;
        public Vector2 scaleDifference;
        public Vector3 textureSize;
        public TextureFormat textureFormat;

        public TextureInformation(byte[] imageData, Vector2 scaleDifference, Vector3 textureSize, TextureFormat textureFormat)
        {
            this.imageData = imageData;
            this.scaleDifference = scaleDifference;
            this.textureSize = textureSize;
            this.textureFormat = textureFormat;
        }
    }

    
}


