using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Assets.AltUnityTester.AltUnityDriver;
using Newtonsoft.Json;
using UnityEngine;


public enum PLayerPrefKeyType { Int = 1, String, Float }

public class AltUnityDriver
{
    public TcpClient Socket;
    private static String tcp_ip = "127.0.0.1";
    private static int tcp_port = 13000;
    private static int BUFFER_SIZE = 1024;
    public AltUnityDriver()
    {

        Socket = new TcpClient();
        Socket.Connect(tcp_ip, tcp_port);
        AltUnityObject.altUnityDriver = this;

    }

    public void Stop()
    {
        Socket.Client.Send(toBytes("closeConnection;&"));
        Thread.Sleep(1000);
        Socket.Close();




    }

    public string Recvall()
    {

        String data = "";
        while (true)
        {
            var bytesReceived = new byte[BUFFER_SIZE];
            Socket.Client.Receive(bytesReceived);
            String part = fromBytes(bytesReceived);
            data += part;
            if (part.Contains("::altend"))
                break;
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
        Socket.Client.Send(toBytes("loadScene;" + scene + ";&"));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }

    public string CallStaticMethods(String typeName, String methodName,
        String parameters, String typeOfParameters = "",String assemblyName="")
    {
        String actionInfo =
            JsonConvert.SerializeObject(new AltUnityObjectAction(typeName, methodName, parameters,typeOfParameters,assemblyName));
        Socket.Client.Send(toBytes("callComponentMethodForObject;" + "" + "; " + actionInfo + "; &"));
        var data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;
    }
    public void DeletePlayerPref()
    {
        Socket.Client.Send(toBytes("deletePlayerPref;&"));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public void DeleteKeyPlayerPref(string keyName)
    {
        Socket.Client.Send(toBytes("deleteKeyPlayerPref;" + keyName + ";&"));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public void SetKeyPlayerPref(string keyName, int valueName)
    {
        Socket.Client.Send(toBytes("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PLayerPrefKeyType.Int + ";&"));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;

        HandleErrors(data);


    }
    public void SetKeyPlayerPref(string keyName, float valueName)
    {
        Socket.Client.Send(toBytes("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PLayerPrefKeyType.Float + ";&"));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public void SetKeyPlayerPref(string keyName, string valueName)
    {
        Socket.Client.Send(toBytes("setKeyPlayerPref;" + keyName + ";" + valueName + ";" + PLayerPrefKeyType.String + ";&"));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);

    }
    public int GetIntKeyPlayerPref(string keyname)
    {
        Socket.Client.Send(toBytes("getKeyPlayerPref;" + keyname + ";" + PLayerPrefKeyType.Int + ";&"));
        var data = Recvall();
        if (!data.Contains("error:")) return Int32.Parse(data);
        HandleErrors(data);
        return 0;

    }
    public float GetFloatKeyPlayerPref(string keyname)
    {
        Socket.Client.Send(toBytes("getKeyPlayerPref;" + keyname + ";" + PLayerPrefKeyType.Float + ";&"));
        var data = Recvall();
        if (!data.Contains("error:")) return Single.Parse(data);
        HandleErrors(data);
        return 0;

    }
    public string GetStringKeyPlayerPref(string keyname)
    {
        Socket.Client.Send(toBytes("getKeyPlayerPref;" + keyname + ";" + PLayerPrefKeyType.String + ";&"));
        var data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;

    }

    public String GetCurrentScene()
    {

        Socket.Client.Send(toBytes("getCurrentScene;&"));
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
        Socket.Client.Send(toBytes("movingTouch;" + vectorStartJson + ";" + vectorEndJson + ";" + duration + ";&"));
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
            Socket.Client.Send(toBytes("swipeFinished;&"));
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
        Socket.Client.Send(toBytes("tapScreen;" + x + ";" + y + ";&"));
        string data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<AltUnityObject>(data);
        HandleErrors(data);
        return null;
    }

    public void Tilt(Vector3 acceleration)
    {
        String accelerationString = JsonConvert.SerializeObject(acceleration, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes("tilt;" + accelerationString + ";&"));
        string data = Recvall();
        if (data.Equals("OK")) return;
        HandleErrors(data);


    }

    public AltUnityObject FindElementWhereNameContains(String name, String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findObjectWhereNameContains;" + name + ";" + cameraName + ";" + enabled + ";&"));
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

    public List<AltUnityObject> GetAllElements(String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findAllObjects;"  + cameraName + ";" + enabled + "&"));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;

    }

    public AltUnityObject FindElement(String name, String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findObjectByName;" + name + ";" + cameraName +";"+enabled+";&"));
        String data = Recvall();
        if (!data.Contains("error:"))
        {
            return JsonConvert.DeserializeObject<AltUnityObject>(data);

        }
        HandleErrors(data);
        return null;
    }

    public List<AltUnityObject> FindElements(String name, String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findObjectsByName;" + name + ";" + cameraName + ";" + enabled + ";&"));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }

    public List<AltUnityObject> FindElementsWhereNameContains(String name, String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findObjectsWhereNameContains;" + name + ";" + cameraName + ";" + enabled + ";&"));
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
           if(!currentScene.Equals(sceneName))
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
                altElement = FindElementWhereNameContains(name,cameraName);
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
        AltUnityObject altElement =null;
        while (time <= timeout)
        {

            try
            {
                altElement = FindElement(name,cameraName);
                Thread.Sleep(Convert.ToInt32(interval * 1000));
                time += interval;
                Debug.Log("Waiting for element " + name + " to not be present");
            }
            catch(Exception)
            { 
                break;
            }

        }

        if (!altElement.Equals(null))
            throw new WaitTimeOutException("Element " + name + " still not found after " + timeout + " seconds");
    }



    public AltUnityObject WaitForElement(String name, String cameraName = "", double timeout = 20, double interval = 0.5)
    {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement = FindElement(name,cameraName);
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

    /// <summary>
    /// Wait until in the scene there is an object with text
    /// </summary>
    /// <param name="name">Name of the object</param>
    /// <param name="text"></param>
    /// <param name="cameraName"></param>
    /// <param name="timeout"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    public AltUnityObject WaitForElementWithText(String name, string text, String cameraName = "", double timeout = 20, double interval = 0.5)
    {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < timeout)
        {
            try
            {
                altElement = FindElement(name,cameraName);
                if(altElement.GetText().Equals(text))
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

    public AltUnityObject FindElementByComponent(String componentName,String assemblyName="", String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findObjectByComponent;" + assemblyName + ";" + componentName + ";" + cameraName +";"+enabled+";&"));
        String data = Recvall();
        if (!data.Contains("error:"))
        {
            return JsonConvert.DeserializeObject<AltUnityObject>(data);
        }
        HandleErrors(data);
        return null;
    }
    /// <summary>
    /// Find all GameObjects that have componentName
    /// </summary>
    /// <param name="componentName">Name of the component by wich is going to search</param>
    /// <returns>List of AltUnityObjects that have component</returns>
    public List<AltUnityObject> FindElementsByComponent(String componentName, String assemblyName = "", String cameraName = "",bool enabled=true)
    {
        Socket.Client.Send(toBytes("findObjectsByComponent;" + assemblyName + ";" + componentName + ";" + cameraName + ";" + enabled + ";&"));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }

    public List<String> GetAllScenes()
    {
        Socket.Client.Send(toBytes("getAllScenes;&"));
        String data = Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<String>>(data);
        HandleErrors(data);
        return null;
    }
    public TextureInformation RecImage()
    {
        var scaleDifference = Recvall();
        var length = Recvall();
        var LongLength = JsonConvert.DeserializeObject<long>(length);
        var textSizeString = Recvall();
        var textSizeVector3 = JsonConvert.DeserializeObject<Vector3>(textSizeString);
        IEnumerable<Byte> data = new List<byte>();
        while (true)
        {

            var bytesReceived = new byte[BUFFER_SIZE];
            Socket.Client.Receive(bytesReceived);
            String part = fromBytes(bytesReceived);
            data = data.Concat(bytesReceived);
            if (part.Contains("::altend"))
                break;
        }

        try
        {
            byte[] start = Encoding.ASCII.GetBytes("altstart::");
            byte[] end = Encoding.ASCII.GetBytes("::altend");

            Byte[] image = data.ToArray();
            var newImage = SubArray(image, start.Length, LongLength);
            using (var msi = new MemoryStream(newImage))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                newImage = mso.ToArray();
            }
            Texture2D texture2D = new Texture2D((int)textSizeVector3.x, (int)textSizeVector3.y, TextureFormat.DXT5, false);
            texture2D.LoadRawTextureData(newImage);
            texture2D.Apply(false);
            return new TextureInformation(texture2D, JsonConvert.DeserializeObject<Vector2>(scaleDifference));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.StackTrace);
        }

        return new TextureInformation();
    }

    public TextureInformation GetScreenshot(Vector2 size = default(Vector2))
    {
        var sizeSerialized = JsonConvert.SerializeObject(size);
        Socket.Client.Send(toBytes("getScreenshot;" + sizeSerialized + ";&"));
        return RecImage();
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
    public static T[] SubArray<T>(T[] data, int index, long length)
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
        public Texture2D texture2D;
        public Vector2 originalSize;

        public TextureInformation(Texture2D texture2D, Vector2 originalSize)
        {
            this.texture2D = texture2D;
            this.originalSize = originalSize;
        }
    }

}


