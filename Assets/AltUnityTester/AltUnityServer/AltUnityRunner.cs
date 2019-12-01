using System;
using System.Linq;
using altunitytester.Assets.AltUnityTester.AltUnityServer;
using Assets.AltUnityTester.AltUnityServer.Commands;
public class AltUnityRunner : UnityEngine.MonoBehaviour, AltIClientSocketHandlerDelegate
{
    enum FindOption
    {
        Name, ContainName, Component
    }
    
    public static AltUnityRunner _altUnityRunner;
    
    public UnityEngine.GameObject AltUnityPopUp;
    public UnityEngine.UI.Image AltUnityIcon;
    public UnityEngine.UI.Text AltUnityPopUpText;
    public bool AltUnityIconPressed=false;
    
    private UnityEngine.Vector3 _position;
    private AltSocketServer _socketServer;

    public static String logMessage="";
    public bool logEnabled;

    private string myPathFile;
    public static System.IO.StreamWriter FileWriter;
    



    public readonly string errorNotFoundMessage = "error:notFound";
    public readonly string errorPropertyNotFoundMessage = "error:propertyNotFound";
    public readonly string errorMethodNotFoundMessage = "error:methodNotFound";
    public readonly string errorComponentNotFoundMessage = "error:componentNotFound";
    public readonly string errorCouldNotPerformOperationMessage = "error:couldNotPerformOperation";
    public readonly string errorCouldNotParseJsonString = "error:couldNotParseJsonString";
    public readonly string errorIncorrectNumberOfParameters = "error:incorrectNumberOfParameters";
    public readonly string errorFailedToParseArguments = "error:failedToParseMethodArguments";
    public readonly string errorObjectWasNotFound = "error:objectNotFound";
    public readonly string errorPropertyNotSet = "error:propertyCannotBeSet";
    public readonly string errorNullRefferenceMessage = "error:nullReferenceException";
    public readonly string errorUnknownError = "error:unknownError";
    public readonly string errorFormatException = "error:formatException";

    public Newtonsoft.Json.JsonSerializerSettings _jsonSettings;

    [UnityEngine.Space] 
    [UnityEngine.SerializeField] private bool _showInputs;
    [UnityEngine.SerializeField] private InputsVisualiser _inputsVisualiser;

    [UnityEngine.Space]

    public bool showPopUp;
    [UnityEngine.SerializeField] private UnityEngine.GameObject AltUnityPopUpCanvas;
    public bool destroyHightlight = false; 
    public int SocketPortNumber = 13000;
    public bool DebugBuildNeeded = true;

    public UnityEngine.Shader outlineShader;
    public UnityEngine.GameObject panelHightlightPrefab;

    public string requestSeparatorString=";";
    public string requestEndingString="&";

    
    public static AltResponseQueue _responseQueue;

    public bool ShowInputs
    {
        get
        {
            return _showInputs;
        }

        set
        {
            _showInputs = value;
        }
    }

    #region MonoBehaviour

    void Awake()
    {
        if (_altUnityRunner == null)
            _altUnityRunner = this;
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        _jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
        _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

        _responseQueue = new AltResponseQueue();

        if (DebugBuildNeeded && !UnityEngine.Debug.isDebugBuild)
        {
            UnityEngine.Debug.Log("AltUnityTester will not run if this is not a Debug/Development build");
        }
        else
        {
            DontDestroyOnLoad(this);
            StartSocketServer();
            UnityEngine.Debug.Log("AltUnity Driver started");
        }
        myPathFile = UnityEngine.Application.persistentDataPath + "/AltUnityTesterLogFile.txt";
        UnityEngine.Debug.Log(myPathFile);
        FileWriter = new System.IO.StreamWriter(myPathFile, true);
        if (showPopUp == false)
        {
            AltUnityPopUpCanvas.SetActive(false);
        }

    }

    
    void Update()
    {
        if (!AltUnityIconPressed)
        {
            if (_socketServer.ClientCount != 0)
            {
                AltUnityPopUp.SetActive(false);
            }
            else
            {
                AltUnityPopUp.SetActive(true);
            }
        }
        if (!_socketServer.IsServerStopped())
        {
            AltUnityIcon.color = UnityEngine.Color.white;
        }
        else
        {
            AltUnityIcon.color = UnityEngine.Color.red;
            AltUnityPopUpText.text = "Server stopped working."+System.Environment.NewLine+" Please restart the server";
        }
        _responseQueue.Cycle();
    }
     void OnApplicationQuit()
    {
        CleanUp();
        FileWriter.Close();
    }
    
    #endregion
    public void CleanUp()
    {
        UnityEngine.Debug.Log("Cleaning up socket server");
        _socketServer.Cleanup();
    }
    
    public void StartSocketServer()
    {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;

        System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        _socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, SocketPortNumber, maxClients, requestEndingString, encoding);

        _socketServer.StartListeningForConnections();
        AltUnityPopUpText.text = "Waiting for connection"+System.Environment.NewLine+"on port " + _socketServer.PortNumber + "...";
        UnityEngine.Debug.Log(string.Format(
            "AltUnity Server at {0} on port {1}",
            _socketServer.LocalEndPoint.Address, _socketServer.PortNumber));
    }
   
    private UnityEngine.Vector3 getObjectScreePosition(UnityEngine.GameObject gameObject, UnityEngine.Camera camera)
    {
        UnityEngine.Canvas canvasParent = gameObject.GetComponentInParent<UnityEngine.Canvas>();
        if (canvasParent != null)
        {
            if (canvasParent.renderMode != UnityEngine.RenderMode.ScreenSpaceOverlay)
            {
                if (gameObject.GetComponent<UnityEngine.RectTransform>() == null)
                {
                    return canvasParent.worldCamera.WorldToScreenPoint(gameObject.transform.position);
                }
                UnityEngine.Vector3[] vector3S = new UnityEngine.Vector3[4];
                gameObject.GetComponent<UnityEngine.RectTransform>().GetWorldCorners(vector3S);
                var center = new UnityEngine.Vector3((vector3S[0].x + vector3S[2].x) / 2, (vector3S[0].y + vector3S[2].y) / 2, (vector3S[0].z + vector3S[2].z) / 2);
                return canvasParent.worldCamera.WorldToScreenPoint(center);
            }
            if (gameObject.GetComponent<UnityEngine.RectTransform>() != null)
            {
                return gameObject.GetComponent<UnityEngine.RectTransform>().position;
            }
            return camera.WorldToScreenPoint(gameObject.transform.position);
        }

        if (gameObject.GetComponent<UnityEngine.Collider>() != null)
        {
            return camera.WorldToScreenPoint(gameObject.GetComponent<UnityEngine.Collider>().bounds.center);
        }

        return camera.WorldToScreenPoint(gameObject.transform.position);
    }

    public AltUnityObject GameObjectToAltUnityObject(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
    {
        int cameraId = -1;
        //if no camera is given it will iterate through all cameras until  found one that sees the object if no camera sees the object it will return the position from the last camera
        //if there is no camera in the scene it will return as scren position x:-1 y=-1, z=-1 and cameraId=-1
        if (camera == null)
        {
            _position = new UnityEngine.Vector3(-1, -1, -1);
            foreach (var camera1 in UnityEngine.Camera.allCameras)
            {
                _position = getObjectScreePosition(altGameObject, camera1);
                cameraId = camera1.GetInstanceID();
                if (_position.x > 0 && _position.y > 0 && _position.x < UnityEngine.Screen.width && _position.y < UnityEngine.Screen.height && _position.z >= 0)//Check if camera sees the object
                {
                    break;
                }
            }
        }
        else
        {
            _position = getObjectScreePosition(altGameObject, camera);
            cameraId = camera.GetInstanceID();

        }
        int parentId = 0;
        if (altGameObject.transform.parent != null)
        {
            parentId = altGameObject.transform.parent.GetInstanceID();
        }


        AltUnityObject altObject = new AltUnityObject(name: altGameObject.name,
                                                      id: altGameObject.GetInstanceID(),
                                                      x: System.Convert.ToInt32(UnityEngine.Mathf.Round(_position.x)),
                                                      y: System.Convert.ToInt32(UnityEngine.Mathf.Round(_position.y)),
                                                      z: System.Convert.ToInt32(UnityEngine.Mathf.Round(_position.z)),//if z is negative object is behind the camera
                                                      mobileY: System.Convert.ToInt32(UnityEngine.Mathf.Round(UnityEngine.Screen.height - _position.y)),
                                                      type: "",
                                                      enabled: altGameObject.activeSelf,
                                                      worldX: altGameObject.transform.position.x,
                                                      worldY: altGameObject.transform.position.y,
                                                      worldZ: altGameObject.transform.position.z,
                                                      idCamera: cameraId,
                                                      transformId: altGameObject.transform.GetInstanceID(),
                                                      parentId: parentId);
        return altObject;
    }

    public void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message) {
        string[] separator = new string[] { requestSeparatorString };
        string[] pieces = message.Split(separator, System.StringSplitOptions.None);
        AltUnityComponent altComponent;
        AltUnityObject altUnityObject;
        string methodParameters;
        UnityEngine.Vector2 size;
        PLayerPrefKeyType option;
        Command command=null;
        
        try
        {
            switch (pieces[0])
            {
                case "findAllObjects":                   
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2];
                    command = new FindAllObjectsCommand(methodParameters);
                    break;
                case "findObjectByName":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindObjectByNameCommand(methodParameters);
                    break;
                case "findObjectWhereNameContains":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindObjectWhereNameContainsCommand(methodParameters);
                    break;
                case "tapObject":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new TapCommand(altUnityObject);
                    break;
                case "findObjectsByName":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindObjectsByNameCommand(methodParameters);
                    break;
                case "findObjectsWhereNameContains":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindObjectsWhereNameContainsCommand(methodParameters);
                    break;
                case "getCurrentScene":
                    command = new GetCurrentSceneCommand();
                    break;
                case "findObjectByComponent":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3] + requestSeparatorString + pieces[4];
                    command = new FindObjectByComponentCommand(methodParameters);
                    break;
                case "findObjectsByComponent":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3] + requestSeparatorString + pieces[4];
                    command = new FindObjectsByComponentCommand(methodParameters);
                    break;
                case "getObjectComponentProperty":
                    command = new GetComponentPropertyCommand(pieces[1], pieces[2]);
                    break;
                case "setObjectComponentProperty":
                    command = new SetObjectComponentPropertyCommand(pieces[1], pieces[2], pieces[3]);
                    break;
                case "callComponentMethodForObject":
                    command = new CallComponentMethodForObjectCommand(pieces[1], pieces[2]);
                    break;
                case "closeConnection":
                    UnityEngine.Debug.Log("Socket connection closed!");
                    _socketServer.StartListeningForConnections();
                    break;
                case "clickEvent":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new ClickEventCommand(altUnityObject);
                    break;
                case "tapScreen":
                    command = new ClickOnScreenAtXyCommand(pieces[1], pieces[2]);
                    break;
                case "dragObject":
                    UnityEngine.Vector2 positionVector2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[2]);
                    command = new DragObjectCommand(positionVector2, altUnityObject);
                    break;
                case "dropObject":
                    UnityEngine.Vector2 positionDropVector2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[2]);
                    command = new DropObjectCommand(positionDropVector2, altUnityObject);
                    break;
                case "pointerUpFromObject":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new PointerUpFromObjectCommand(altUnityObject);
                    break;
                case "pointerDownFromObject":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new PointerDownFromObjectCommand(altUnityObject);
                    break;
                case "pointerEnterObject":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new PointerEnterObjectCommand(altUnityObject);
                    break;
                case "pointerExitObject":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new PointerExitObjectCommand(altUnityObject);
                    break;
                case "tilt":
                    UnityEngine.Vector3 vector3 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector3>(pieces[1]);
                    command = new TiltCommand(vector3);
                    break;
                case "movingTouch":
                    UnityEngine.Vector2 start2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    UnityEngine.Vector2 end2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[2]);
                    command = new SetMovingTouchCommand(start2, end2, pieces[3]);
                    break;
                case "loadScene":
                    command = new Assets.AltUnityTester.AltUnityServer.Commands.LoadSceneCommand(pieces[1]);
                    break;
                case "setTimeScale":
                    float timeScale = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[1]);
                    command = new SetTimeScaleCommand(timeScale);
                    break;
                case "getTimeScale":
                    command = new GetTimeScaleCommand();
                    break;
                case "deletePlayerPref":
                    command = new DeletePlayerPrefCommand();
                    break;
                case "deleteKeyPlayerPref":
                    command = new DeleteKeyPlayerPrefCommand(pieces[1]);
                    break;
                case "setKeyPlayerPref":
                    option = (PLayerPrefKeyType)System.Enum.Parse(typeof(PLayerPrefKeyType), pieces[3]);
                    command = new SetKeyPlayerPrefCommand(option, pieces[1], pieces[2]);
                    break;
                case "getKeyPlayerPref":
                    option = (PLayerPrefKeyType)System.Enum.Parse(typeof(PLayerPrefKeyType), pieces[2]);
                    command = new GetKeyPlayerPrefCommand(option, pieces[1]);
                    break;
                case "actionFinished":
                    command = new ActionFinishedCommand();
                    break;
                case "getAllComponents":
                    command = new GetAllComponentsCommand(pieces[1]);
                    break;
                case "getAllFields":
                    altComponent = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityComponent>(pieces[2]);
                    command = new GetAllFieldsCommand(pieces[1], altComponent);
                    break;
                case "getAllMethods":
                    altComponent = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityComponent>(pieces[1]);
                    command = new GetAllMethodsCommand(altComponent);
                    break;
                case "getAllScenes":
                    command = new GetAllScenesCommand();
                    break;
                case "getAllCameras":
                    command = new GetAllCamerasCommand();
                    break;
                case "getScreenshot":
                    size = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    command = new GetScreenshotCommand(size,handler);
                    break;
                case "hightlightObjectScreenshot":
                    var id = System.Convert.ToInt32(pieces[1]);
                    size = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[3]);
                    command = new HighlightSelectedObjectCommand(id, pieces[2], size, handler);
                    break;
                case "hightlightObjectFromCoordinatesScreenshot":
                    var coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    size = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[3]);
                    command = new HightlightObjectFromCoordinatesCommand(coordinates, pieces[2], size, handler);
                    break;
                case "pressKeyboardKey":
                    var piece = pieces[1];
                    UnityEngine.KeyCode keycode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), piece);
                    float power = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[2]);
                    float duration = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[3]);
                    command = new HoldButtonCommand(keycode, power, duration);
                    break;
                case "moveMouse":
                    UnityEngine.Vector2 location = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    duration = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[2]);
                    command = new MoveMouseCommand(location, duration);
                    break;
                case "scrollMouse":
                    var scrollValue = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[1]);
                    duration = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[2]);
                    command = new ScrollMouseCommand(scrollValue, duration);
                    break;
                case "findObject":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindObjectCommand(methodParameters);
                    break;
                case "findObjects":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindObjectsCommand(methodParameters);
                    break;
                case "findActiveObjectByName":
                    methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                    command = new FindActiveObjectsByNameCommand(methodParameters);
                    break;
                case "enableLogging":
                    var enableLogging = bool.Parse(pieces[1]);
                    command = new EnableLoggingCommand(enableLogging);
                    break;

                case "getText":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new GetTextCommand(altUnityObject);
                    break;
                case "setText":
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    command = new SetTextCommand(altUnityObject, pieces[2]);
                    break;
                case "getPNGScreenshot":
                    command = new GetScreenshotPNGCommand(handler);
                    break;


                default:
                    command = new UnknowStringCommand();
                    break;
            }
        }
        catch (Newtonsoft.Json.JsonException exception)
        {
            UnityEngine.Debug.Log(exception);
            handler.SendResponse(errorCouldNotParseJsonString);
        }
        if (command != null)
        {
            command.SendResponse(handler);
        }           
    }

    public void LogMessage(string logMessage)
    {
        if (logEnabled)
        {
            AltUnityRunner.logMessage += System.DateTime.Now + ":" + logMessage + System.Environment.NewLine;
            FileWriter.WriteLine(System.DateTime.Now + ":" + logMessage);
            UnityEngine.Debug.Log(logMessage);
        }
    }

    public static UnityEngine.GameObject[] GetDontDestroyOnLoadObjects()
    {
        UnityEngine.GameObject temp = null;
        try
        {
            temp = new UnityEngine.GameObject();
            DontDestroyOnLoad(temp);
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                DestroyImmediate(temp);
        }
    }        
   
    public void ServerRestartPressed()
    {
        AltUnityIconPressed = false;
        _socketServer.Cleanup();
        StartSocketServer();
        AltUnityPopUp.SetActive(true);
    }
    
    public void IconPressed()
    {
        AltUnityPopUp.SetActive(!AltUnityPopUp.activeSelf);
        AltUnityIconPressed = !AltUnityIconPressed;
    }

    public static UnityEngine.GameObject GetGameObject(AltUnityObject altUnityObject)
    {
        foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
        {
            if (gameObject.GetInstanceID() == altUnityObject.id)
                return gameObject;
        }
        throw new Assets.AltUnityTester.AltUnityDriver.NotFoundException("Object not found");
    }
    
    public static UnityEngine.GameObject GetGameObject(int objectId)
    {
        foreach (UnityEngine.GameObject gameObject in FindObjectsOfType<UnityEngine.GameObject>())
        {
            if (gameObject.GetInstanceID() == objectId)
                return gameObject;
        }
        throw new Assets.AltUnityTester.AltUnityDriver.NotFoundException("Object not found");
    }       

    public UnityEngine.Camera FoundCameraById(int id)
    {
        foreach (var camera in UnityEngine.Camera.allCameras)
        {
            if (camera.GetInstanceID() == id)
                return camera;
        }

        return null;
    }
    
    public System.Collections.IEnumerator HighLightSelectedObjectCorutine(UnityEngine.GameObject gameObject, UnityEngine.Color color, float width, UnityEngine.Vector2 size, AltClientSocketHandler handler)
    {
        destroyHightlight = false;
        UnityEngine.Renderer renderer = gameObject.GetComponent<UnityEngine.Renderer>();
        System.Collections.Generic.List<UnityEngine.Shader> originalShaders = new System.Collections.Generic.List<UnityEngine.Shader>();
        if (renderer != null)
        {
            foreach (var material in renderer.materials)
            {
                originalShaders.Add(material.shader);
                material.shader = outlineShader;
                material.SetColor("_OutlineColor", color);
                material.SetFloat("_OutlineWidth", width);
            }
            yield return null;
            new GetScreenshotCommand(size, handler).Execute();
            yield return null;
            for (var i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i].shader = originalShaders[0];
            }
        }
        else
        {
            var rectTransform = gameObject.GetComponent<UnityEngine.RectTransform>();
            if (rectTransform != null)
            {
                var panelHighlight = Instantiate(panelHightlightPrefab, rectTransform);
                panelHighlight.GetComponent<UnityEngine.UI.Image>().color = color;
                yield return null;
                new GetScreenshotCommand(size,handler).Execute();
                while (!destroyHightlight)
                    yield return null;
                Destroy(panelHighlight);
                destroyHightlight = false;
            }
            else
            {
                var response= new GetScreenshotCommand(size,handler).Execute();
                handler.SendResponse(response);  
            }
        }
    }
    
    public System.Collections.IEnumerator TakeTexturedScreenshot(UnityEngine.Vector2 size, AltClientSocketHandler handler) {
        yield return new UnityEngine.WaitForEndOfFrame();
        var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();

        var response=new ScreenshotReadyCommand(screenshot, size).Execute();
        handler.SendResponse(response);
    }
    public System.Collections.IEnumerator TakeScreenshot(AltClientSocketHandler handler)
    {
        yield return new UnityEngine.WaitForEndOfFrame();
        var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
        var bytesPNG = UnityEngine.ImageConversion.EncodeToPNG(screenshot);
        var pngAsString = Convert.ToBase64String(bytesPNG);

        handler.SendResponse(pngAsString);
    }

    public void ShowClick(UnityEngine.Vector2 position)
    {
        if (!_showInputs || _inputsVisualiser == null)
            return;
        
        _inputsVisualiser.ShowClick(position);
    }
    
    public int ShowInput(UnityEngine.Vector2 position, int markId = -1)
    {
        if (!_showInputs || _inputsVisualiser == null)
            return -1;

        return _inputsVisualiser.ShowContinuousInput(position, markId);
    }
    
    public static void CopyTo(System.IO.Stream src, System.IO.Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }
    
    public static byte[] CompressScreenshot(byte[] screenshotSerialized)
    {
        using (var memoryStreamInput = new System.IO.MemoryStream(screenshotSerialized))
        using (var memoryStreamOutout = new System.IO.MemoryStream())
        {
            using (var gZipStream = new Unity.IO.Compression.GZipStream(memoryStreamOutout, Unity.IO.Compression.CompressionMode.Compress))
            {
                CopyTo(memoryStreamInput, gZipStream);
            }

            return memoryStreamOutout.ToArray();
        }
    }
    static bool ByteArrayCompare(byte[] a1, byte[] a2)
    {
        if (a1.Length != a2.Length)
            return false;

        for (int i = 0; i < a1.Length; i++)
            if (a1[i] != a2[i])
                return false;

        return true;
    }
}
