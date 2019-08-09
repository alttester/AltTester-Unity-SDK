using System.Linq;

public class AltUnityRunner : UnityEngine.MonoBehaviour, AltIClientSocketHandlerDelegate
{

    public UnityEngine.GameObject AltUnityPopUp;
    public UnityEngine.UI.Image AltUnityIcon;
    public UnityEngine.UI.Text AltUnityPopUpText;
    public bool AltUnityIconPressed=false;

    private static AltUnityRunner _altUnityRunner;
    private UnityEngine.Vector3 _position;
    private AltSocketServer _socketServer;

    private readonly string errorNotFoundMessage = "error:notFound";
    private readonly string errorPropertyNotFoundMessage = "error:propertyNotFound";
    private readonly string errorMethodNotFoundMessage = "error:methodNotFound";
    private readonly string errorComponentNotFoundMessage = "error:componentNotFound";
    private readonly string errorCouldNotPerformOperationMessage = "error:couldNotPerformOperation";
    private readonly string errorCouldNotParseJsonString = "error:couldNotParseJsonString";
    private readonly string errorIncorrectNumberOfParameters = "error:incorrectNumberOfParameters";
    private readonly string errorFailedToParseArguments = "error:failedToParseMethodArguments";
    private readonly string errorObjectWasNotFound = "error:objectNotFound";
    private readonly string errorPropertyNotSet = "error:propertyCannotBeSet";
    private readonly string errorNullRefferenceMessage = "error:nullRefferenceException";
    private readonly string errorUnknownError = "error:unknownError";
    private readonly string errorFormatException = "error:formatException";

    private Newtonsoft.Json.JsonSerializerSettings _jsonSettings;

    private bool destroyHightlight = false; 

    enum FindOption
    {
        Name, ContainName, Component
    }
    public int SocketPortNumber = 13000;
    public bool DebugBuildNeeded = true;

    public UnityEngine.Shader outlineShader;
    public UnityEngine.GameObject panelHightlightPrefab;

    public string requestSeparatorString=";";
    public string requestEndingString="&";

    

    private static AltResponseQueue _responseQueue;

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

        AltUnityEvents.Instance.FindObjectByName.AddListener(FindObjectByName);
        AltUnityEvents.Instance.FindObjectWhereNameContains.AddListener(FindObjectWhereNameContains);
        AltUnityEvents.Instance.FindObjectByComponent.AddListener(FindObjectByComponent);

        AltUnityEvents.Instance.FindObjectsByName.AddListener(FindObjectsByName);
        AltUnityEvents.Instance.FindObjectsWhereNameContains.AddListener(FindObjectsWhereNameContains);
        AltUnityEvents.Instance.FindObjectsByComponent.AddListener(FindObjectsByComponent);

        AltUnityEvents.Instance.GetAllObjects.AddListener(GetAllObjects);
        AltUnityEvents.Instance.GetCurrentScene.AddListener(GetCurrentScene);


        AltUnityEvents.Instance.ClickEvent.AddListener(ClickEvent);
        AltUnityEvents.Instance.TapScreen.AddListener(ClickOnScreenAtXy);
        AltUnityEvents.Instance.Tap.AddListener(Tap);
        AltUnityEvents.Instance.GetComponentProperty.AddListener(GetObjectComponentProperty);
        AltUnityEvents.Instance.SetComponentProperty.AddListener(SetObjectComponentProperty);
        AltUnityEvents.Instance.CallComponentMethod.AddListener(CallComponentMethodForObject);

        AltUnityEvents.Instance.CloseConnection.AddListener(CloseConnection);
        AltUnityEvents.Instance.UnknownString.AddListener(UnknownString);

        AltUnityEvents.Instance.DragObject.AddListener(DragObject);
        AltUnityEvents.Instance.DropObject.AddListener(DropObject);
        AltUnityEvents.Instance.PointerUp.AddListener(PointerUpFromObject);
        AltUnityEvents.Instance.PointerDown.AddListener(PointerDownFromObject);
        AltUnityEvents.Instance.PointerEnter.AddListener(PointerEnterObject);
        AltUnityEvents.Instance.PointerExit.AddListener(PointerExitObject);
#if ALTUNITYTESTER
        
        AltUnityEvents.Instance.Tilt.AddListener(Tilt);
        AltUnityEvents.Instance.SetMovingTouch.AddListener(SetMovingTouch);
        AltUnityEvents.Instance.ActionFinished.AddListener(ActionFinished);
        AltUnityEvents.Instance.HoldButton.AddListener(HoldButton);
        AltUnityEvents.Instance.Scroll.AddListener(ScrollMouse);
        AltUnityEvents.Instance.MoveMouse.AddListener(MoveMouse);
#endif

        AltUnityEvents.Instance.LoadScene.AddListener(LoadScene);
        AltUnityEvents.Instance.SetKeyPlayerPref.AddListener(SetKeyPlayerPref);
        AltUnityEvents.Instance.GetKeyPlayerPref.AddListener(GetKeyPlayerPref);
        AltUnityEvents.Instance.DeleteKeyPlayerPref.AddListener(DeleteKeyPlayerPref);
        AltUnityEvents.Instance.DeletePlayerPref.AddListener(DeletePlayerPref);

        AltUnityEvents.Instance.GetAllComponents.AddListener(GetAllComponents);
        AltUnityEvents.Instance.GetAllMethods.AddListener(GetAllMethods);
        AltUnityEvents.Instance.GetAllFields.AddListener(GetAllFields);
        AltUnityEvents.Instance.GetAllScenes.AddListener(GetAllScenes);
        AltUnityEvents.Instance.GetAllCameras.AddListener(GetAllCameras);

        AltUnityEvents.Instance.GetScreenshot.AddListener(GetScreenshot);
        AltUnityEvents.Instance.HighlightObjectScreenshot.AddListener(HighLightSelectedObject);
        AltUnityEvents.Instance.HighlightObjectFromCoordinates.AddListener(HightObjectFromCoordinates);
        AltUnityEvents.Instance.ScreenshotReady.AddListener(ScreenshotReady);

        AltUnityEvents.Instance.SetTimeScale.AddListener(SetTimeScale);
        AltUnityEvents.Instance.GetTimeScale.AddListener(GetTimeScale);

        



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

    }

    

    public void StartSocketServer()
    {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;

        System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        _socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, SocketPortNumber, maxClients, requestEndingString, encoding);

        _socketServer.StartListeningForConnections();
        AltUnityPopUpText.text = "Waiting for connection"+System.Environment.NewLine+"on port: " + _socketServer.PortNumber + " !";
        UnityEngine.Debug.Log(string.Format(
            "AltUnity Server at {0} on port {1}",
            _socketServer.LocalEndPoint.Address, _socketServer.PortNumber));
    }

    void OnApplicationQuit()
    {
        CleanUp();
    }

    public void CleanUp()
    {
        UnityEngine.Debug.Log("Cleaning up socket server");
        _socketServer.Cleanup();
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


    private AltUnityObject GameObjectToAltUnityObject(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
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
        switch (pieces[0])
        {
            case "findAllObjects":
                UnityEngine.Debug.Log("all objects requested");
                methodParameters = pieces[1] + requestSeparatorString + pieces[2];
                AltUnityEvents.Instance.GetAllObjects.Invoke(methodParameters, handler);
                break;
            case "findObjectByName":
                UnityEngine.Debug.Log("find object by name " + pieces[1]);
                UnityEngine.Debug.Log(pieces.Length);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectByName.Invoke(methodParameters, handler);
                break;
            case "findObjectWhereNameContains":
                UnityEngine.Debug.Log("find object where name contains:" + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectWhereNameContains.Invoke(methodParameters, handler);
                break;
            case "tapObject":
                try
                {
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    UnityEngine.Debug.Log("tapped object by name " + altUnityObject.name);
                    AltUnityEvents.Instance.Tap.Invoke(altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "findObjectsByName":
                UnityEngine.Debug.Log("find multiple objects by name " + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectsByName.Invoke(methodParameters, handler);
                break;
            case "findObjectsWhereNameContains":
                UnityEngine.Debug.Log("find objects where name contains:" + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectsWhereNameContains.Invoke(methodParameters, handler);
                break;
            case "getCurrentScene":
                UnityEngine.Debug.Log("get current scene");
                AltUnityEvents.Instance.GetCurrentScene.Invoke(handler);
                break;
            case "findObjectByComponent":
                UnityEngine.Debug.Log("find object by component " + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3] + requestSeparatorString + pieces[4];
                AltUnityEvents.Instance.FindObjectByComponent.Invoke(methodParameters, handler);
                break;
            case "findObjectsByComponent":
                UnityEngine.Debug.Log("find objects by component " + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3] + requestSeparatorString + pieces[4];
                AltUnityEvents.Instance.FindObjectsByComponent.Invoke(methodParameters, handler);
                break;
            case "getObjectComponentProperty":
                UnityEngine.Debug.Log("get property " + pieces[2] + " for object " + pieces[1]);
                AltUnityEvents.Instance.GetComponentProperty.Invoke(pieces[1], pieces[2], handler);
                break;
            case "setObjectComponentProperty":
                UnityEngine.Debug.Log("set property " + pieces[2] + " to value: " + pieces[3] + " for object " + pieces[1]);
                AltUnityEvents.Instance.SetComponentProperty.Invoke(pieces[1], pieces[2], pieces[3], handler);
                break;
            case "callComponentMethodForObject":
                UnityEngine.Debug.Log("call action " + pieces[2] + " for object " + pieces[1]);
                AltUnityEvents.Instance.CallComponentMethod.Invoke(pieces[1], pieces[2], handler);
                break;
            case "closeConnection":
                UnityEngine.Debug.Log("Socket connection closed!");
                AltUnityEvents.Instance.CloseConnection.Invoke(handler);
                break;
            case "clickEvent":
                UnityEngine.Debug.Log("ClickEvent on " + pieces[1]);
                try
                {
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.ClickEvent.Invoke(altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }

                break;
            case "tapScreen":
                UnityEngine.Debug.Log("Screen tapped at X:" + pieces[1] + " Y:" + pieces[2]);
                AltUnityEvents.Instance.TapScreen.Invoke(pieces[1], pieces[2], handler);
                break;
            case "dragObject":
                try
                {
                    UnityEngine.Debug.Log("Drag object");
                    UnityEngine.Vector2 positionVector2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[2]);
                    AltUnityEvents.Instance.DragObject.Invoke(positionVector2, altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "dropObject":
                try
                {
                    UnityEngine.Debug.Log("Drop object");
                    UnityEngine.Vector2 positionDropVector2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[2]);
                    AltUnityEvents.Instance.DropObject.Invoke(positionDropVector2, altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "pointerUpFromObject":
                try
                {
                    UnityEngine.Debug.Log("PointerUp");
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerUp.Invoke(altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "pointerDownFromObject":
                try
                {
                    UnityEngine.Debug.Log("PointerDown");
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerDown.Invoke(altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;

            case "pointerEnterObject":
                try
                {
                    UnityEngine.Debug.Log("PointerEnter");
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerEnter.Invoke(altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "pointerExitObject":
                try
                {
                    UnityEngine.Debug.Log("PointerExit");
                    altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerExit.Invoke(altUnityObject, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;

            case "tilt":
                try
                {
                    UnityEngine.Debug.Log("Tilt");
                    UnityEngine.Vector3 vector3 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector3>(pieces[1]);
                    AltUnityEvents.Instance.Tilt.Invoke(vector3, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;


            case "movingTouch":
                try
                {
                    UnityEngine.Debug.Log("Touch");
                    UnityEngine.Vector2 start2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                    UnityEngine.Vector2 end2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[2]);
                    AltUnityEvents.Instance.SetMovingTouch.Invoke(start2, end2, pieces[3], handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "loadScene":
                UnityEngine.Debug.Log("LoadScene");
                AltUnityEvents.Instance.LoadScene.Invoke(pieces[1], handler);
                break;
            case "setTimeScale":
                UnityEngine.Debug.Log("SetTimeScale");
                float timeScale = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[1]);
                AltUnityEvents.Instance.SetTimeScale.Invoke(timeScale, handler);
                break;
            case "getTimeScale":
                UnityEngine.Debug.Log("GetTimeScale");
                AltUnityEvents.Instance.GetTimeScale.Invoke(handler);
                break;
            case "deletePlayerPref":
                UnityEngine.Debug.Log("deletePlayerPref");
                AltUnityEvents.Instance.DeletePlayerPref.Invoke(handler);
                break;
            case "deleteKeyPlayerPref":
                UnityEngine.Debug.Log("deleteKeyPlayerPref");
                AltUnityEvents.Instance.DeleteKeyPlayerPref.Invoke(pieces[1], handler);
                break;
            case "setKeyPlayerPref":
                try
                {
                    UnityEngine.Debug.Log("setKeyPlayerPref");
                    option = (PLayerPrefKeyType)System.Enum.Parse(typeof(PLayerPrefKeyType), pieces[3]);
                    AltUnityEvents.Instance.SetKeyPlayerPref.Invoke(pieces[1], pieces[2], option, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "getKeyPlayerPref":
                try
                {
                    UnityEngine.Debug.Log("getKeyPlayerPref");
                    option = (PLayerPrefKeyType)System.Enum.Parse(typeof(PLayerPrefKeyType), pieces[2]);
                    AltUnityEvents.Instance.GetKeyPlayerPref.Invoke(pieces[1], option, handler);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    UnityEngine.Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "actionFinished":
                UnityEngine.Debug.Log("actionFinished");
                AltUnityEvents.Instance.ActionFinished.Invoke(handler);
                break;
            case "getAllComponents":
                UnityEngine.Debug.Log("GetAllComponents");
                AltUnityEvents.Instance.GetAllComponents.Invoke(pieces[1], handler);
                break;
            case "getAllFields":
                UnityEngine.Debug.Log("getAllFields");
                altComponent = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityComponent>(pieces[2]);
                AltUnityEvents.Instance.GetAllFields.Invoke(pieces[1], altComponent, handler);
                break;
            case "getAllMethods":
                UnityEngine.Debug.Log("getAllMethods");
                altComponent = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityComponent>(pieces[1]);
                AltUnityEvents.Instance.GetAllMethods.Invoke(altComponent, handler);
                break;
            case "getAllScenes":
                UnityEngine.Debug.Log("getAllScenes");
                AltUnityEvents.Instance.GetAllScenes.Invoke(handler);
                break;
            case "getAllCameras":
                UnityEngine.Debug.Log("getAllCameras");
                AltUnityEvents.Instance.GetAllCameras.Invoke(handler);
                break;
            case "getScreenshot":
                UnityEngine.Debug.Log("getScreenshot" + pieces[1]);
                //                var size = new UnityEngine.Vector2(System.Convert.ToInt32(pieces[1]),System.Convert.ToInt32(pieces[2]));
                size = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                AltUnityEvents.Instance.GetScreenshot.Invoke(size, handler);
                break;
            case "hightlightObjectScreenshot":
                UnityEngine.Debug.Log("HightlightObject");
                var id = System.Convert.ToInt32(pieces[1]);
                size = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[3]);
                AltUnityEvents.Instance.HighlightObjectScreenshot.Invoke(id, pieces[2], size, handler);
                break;
            case "hightlightObjectFromCoordinatesScreenshot":
                UnityEngine.Debug.Log("HightlightObject");
                var coordinates = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                size = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[3]);
                AltUnityEvents.Instance.HighlightObjectFromCoordinates.Invoke(coordinates, pieces[2], size, handler);
                break;
            case "pressKeyboardKey":
                UnityEngine.Debug.Log("pressKeyboardKey");
                var piece1 = pieces[1];
                UnityEngine.KeyCode keycode = (UnityEngine.KeyCode)System.Enum.Parse(typeof(UnityEngine.KeyCode), piece1);
                float power = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[2]);
                float duration = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[3]);
                AltUnityEvents.Instance.HoldButton.Invoke(keycode,power, duration, handler);
                break;
            case "moveMouse":
                UnityEngine.Debug.Log("moveMouse");
                UnityEngine.Vector2 location = Newtonsoft.Json.JsonConvert.DeserializeObject<UnityEngine.Vector2>(pieces[1]);
                duration = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[2]);
                AltUnityEvents.Instance.MoveMouse.Invoke(location, duration, handler);
                break;
            case "scrollMouse":
                UnityEngine.Debug.Log("scrollMouse");
                var scrollValue = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[1]);
                duration = Newtonsoft.Json.JsonConvert.DeserializeObject<float>(pieces[2]);
                AltUnityEvents.Instance.Scroll.Invoke(scrollValue, duration, handler);
                break;

            default:
                AltUnityEvents.Instance.UnknownString.Invoke(handler);
                break;
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
    private UnityEngine.GameObject FindObjectInScene(string objectName, bool enabled)
    {
        string[] pathList = objectName.Split('/');
        UnityEngine.GameObject foundGameObject = null;
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            foreach (UnityEngine.GameObject obj in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
            {
                foundGameObject = CheckPath(obj, pathList, 0, enabled);
                if (foundGameObject != null)
                    return foundGameObject;
                else
                {
                    foundGameObject = CheckChildren(obj, pathList, enabled);
                    if (foundGameObject != null)
                        return foundGameObject;
                }
            }
        }
        foreach (var destroyOnLoadObject in GetDontDestroyOnLoadObjects())
        {
            foundGameObject = CheckPath(destroyOnLoadObject, pathList, 0, enabled);
            if (foundGameObject != null)
                return foundGameObject;
            else
            {
                foundGameObject = CheckChildren(destroyOnLoadObject, pathList, enabled);
                if (foundGameObject != null)
                    return foundGameObject;
            }
        }
        return foundGameObject;
    }
    private UnityEngine.GameObject CheckChildren(UnityEngine.GameObject obj, string[] pathList, bool enabled)
    {
        UnityEngine.GameObject objectReturned = null;
        foreach (UnityEngine.Transform childrenTransform in obj.transform)
        {
            objectReturned = CheckPath(childrenTransform.gameObject, pathList, 0, enabled);
            if (objectReturned != null)
                return objectReturned;
            else
            {
                objectReturned = CheckChildren(childrenTransform.gameObject, pathList, enabled);
                if (objectReturned != null)
                    return objectReturned;
            }
        }
        return objectReturned;
    }
    private UnityEngine.GameObject CheckPath(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
    {
        int option = 1;
        if (pathList[pathListStep].Equals(".."))
            option = 2;
        else
            if (pathList[pathListStep].StartsWith("id("))
            option = 3;
        switch (option)
        {
            case 2:

                if (pathListStep == pathList.Length - 1)
                {
                    if (obj.transform.parent == null || (enabled && obj.activeInHierarchy == false)) return null;
                    return obj.transform.parent.gameObject;
                }
                else
                {
                    int nextStep = pathListStep + 1;
                    return CheckPath(obj.transform.parent.gameObject, pathList, nextStep, enabled);
                }
            case 3:
                var id = System.Convert.ToInt32(pathList[pathListStep].Substring(3, pathList[pathListStep].Length - 4));
                if (obj.GetInstanceID() != id)
                {
                    return null;
                }
                else
                {
                    return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                }
            default:
                if (!obj.name.Equals(pathList[pathListStep]))
                    return null;
                else
                {
                    return CheckNextElementInPath(obj, pathList, pathListStep, enabled);
                }
        }
    }
    private UnityEngine.GameObject CheckNextElementInPath(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
    {
        if (pathListStep == pathList.Length - 1)
            if (enabled && obj.activeInHierarchy == false) return null;
            else
            {
                return obj;
            }
        else
        {
            int nextStep = pathListStep + 1;
            if (pathList[nextStep].Equals(".."))
            {
                var objectReturned = CheckPath(obj, pathList, nextStep, enabled);
                if (objectReturned != null)
                    return objectReturned;
            }
            foreach (UnityEngine.Transform childrenObject in obj.transform)
            {
                var objectReturned = CheckPath(childrenObject.gameObject, pathList, nextStep, enabled);
                if (objectReturned != null)
                    return objectReturned;
            }
            return null;
        }
    }
    private System.Collections.Generic.List<UnityEngine.GameObject> FindObjectsInScene(string objectName, bool enabled)
    {
        System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
        string[] pathList = objectName.Split('/');
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            foreach (UnityEngine.GameObject obj in UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).GetRootGameObjects())
            {
                System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(obj.gameObject, pathList, 0, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
                listGameObjects = CheckChildrenForMultipleElements(obj.gameObject, pathList, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
            }
        }
        foreach (var destroyOnLoadObject in GetDontDestroyOnLoadObjects())
        {
            System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(destroyOnLoadObject.gameObject, pathList, 0, enabled);
            if (listGameObjects != null)
                objectsFound.AddRange(listGameObjects);
            listGameObjects = CheckChildrenForMultipleElements(destroyOnLoadObject.gameObject, pathList, enabled);
            objectsFound.AddRange(listGameObjects);
        }
        return objectsFound;
    }
    private System.Collections.Generic.List<UnityEngine.GameObject> CheckChildrenForMultipleElements(UnityEngine.GameObject obj, string[] pathList, bool enabled)
    {
        System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
        foreach (UnityEngine.Transform childrenTransform in obj.transform)
        {
            System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(childrenTransform.gameObject, pathList, 0, enabled);
            if (listGameObjects != null)
                objectsFound.AddRange(listGameObjects);
            listGameObjects = CheckChildrenForMultipleElements(childrenTransform.gameObject, pathList, enabled);
            if (listGameObjects != null)
                objectsFound.AddRange(listGameObjects);

        }
        return objectsFound;
    }
    private System.Collections.Generic.List<UnityEngine.GameObject> CheckPathForMultipleElements(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
    {
        System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
        int option = 1;
        if (pathList[pathListStep].Equals(".."))
            option = 2;
        else
            if (pathList[pathListStep].StartsWith("id("))
            option = 3;
        switch (option)
        {
            case 2:
                if (pathListStep == pathList.Length - 1)
                {
                    if (obj.transform.parent == null || (enabled && obj.activeInHierarchy == false)) return null;
                    objectsFound.Add(obj.transform.parent.gameObject);
                    return objectsFound;
                }
                else
                {
                    int nextStep = pathListStep + 1;
                    return CheckPathForMultipleElements(obj.transform.parent.gameObject, pathList, nextStep, enabled);
                }
            case 3:
                var id = System.Convert.ToInt32(pathList[pathListStep].Substring(3, pathList[pathListStep].Length - 4));
                if (obj.GetInstanceID() != id)
                {
                    return null;
                }
                else
                {
                    return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                }
            default:
                if (!(obj.name.Equals(pathList[pathListStep]) || (pathList[pathListStep].Equals("") && pathList.Length == 1)))
                    return null;
                else
                {
                    return CheckNextElementInPathForMultipleElements(obj, pathList, pathListStep, enabled);
                }
        }
    }
    private System.Collections.Generic.List<UnityEngine.GameObject> CheckNextElementInPathForMultipleElements(UnityEngine.GameObject obj, string[] pathList, int pathListStep, bool enabled)
    {
        System.Collections.Generic.List<UnityEngine.GameObject> objectsFound = new System.Collections.Generic.List<UnityEngine.GameObject>();
        if (pathListStep == pathList.Length - 1)
            if (enabled && obj.activeInHierarchy == false) return null;
            else
            {
                objectsFound.Add(obj);
                return objectsFound;
            }
        else
        {
            int nextStep = pathListStep + 1;
            if (pathList[nextStep].Equals(".."))
            {
                System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(obj, pathList, nextStep, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
                return objectsFound;
            }
            foreach (UnityEngine.Transform childrenObject in obj.transform)
            {
                System.Collections.Generic.List<UnityEngine.GameObject> listGameObjects = CheckPathForMultipleElements(childrenObject.gameObject, pathList, nextStep, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
            }
            return objectsFound;
        }
    }




    private void FindObjectByName(string stringSent, AltClientSocketHandler handler) {
        var pieces = stringSent.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = System.Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                
                UnityEngine.GameObject foundGameObject = FindObjectInScene(objectName, enabled);
                if (foundGameObject != null)
                {
                    if (cameraName.Equals(""))
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject));
                    else
                    {
                        UnityEngine.Camera camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                        response = camera == null ? errorNotFoundMessage : Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject, camera));
                    }
                }
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }

        });
    }

    private void FindObjectWhereNameContains(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = System.Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.Camera camera=null;
                if(cameraName!=null){
                    camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                }
                foreach (UnityEngine.GameObject testableObject in FindObjectsOfType<UnityEngine.GameObject>())
                {
                    if (testableObject.name.Contains(objectName))
                    {
                        response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(testableObject,camera));
                        break;
                    }
                }
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }

            handler.SendResponse(response);
        });

    }

    private void FindObjectByComponent(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);
        string assemblyName = pieces[0];
        string componentTypeName = pieces[1];
        string cameraName = pieces[2];
        bool enabled = System.Convert.ToBoolean(pieces[3]);
        
        _responseQueue.ScheduleResponse(delegate
        {

            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.Camera camera = null;
                if (cameraName != null)
                {
                    camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                }
                System.Type componentType = GetType(componentTypeName, assemblyName);
                if (componentType != null)
                {
                    foreach (UnityEngine.GameObject testableObject in FindObjectsOfType<UnityEngine.GameObject>())
                    {
                        if (testableObject.GetComponent(componentType) != null)
                        {
                            var foundObject = testableObject;
                            response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundObject,camera));
                            break;
                        }
                    }
                }
                else
                {
                    response = errorComponentNotFoundMessage;
                }
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }

        });

    }

    private void FindObjectsByName(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = System.Convert.ToBoolean(pieces[2]);
        
        _responseQueue.ScheduleResponse(delegate
        {
            UnityEngine.Camera camera=null;
            if(cameraName!=null){
                camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
            }
            string response = errorNotFoundMessage;
            try
            {
                System.Collections.Generic.List<AltUnityObject> foundObjects = new System.Collections.Generic.List<AltUnityObject>();
                foreach (UnityEngine.GameObject testableObject in FindObjectsInScene(objectName, enabled))
                {
                    foundObjects.Add(GameObjectToAltUnityObject(testableObject, camera));
                }

                response = Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }
        });

    }

    private void FindObjectsByComponent(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);
        string assemblyName = pieces[0];
        string componentTypeName = pieces[1];
        string cameraName = pieces[2];
        bool enabled = System.Convert.ToBoolean(pieces[3]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.Camera camera=null;
                if(cameraName!=null){
                    camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                }
                System.Collections.Generic.List<AltUnityObject> foundObjects = new System.Collections.Generic.List<AltUnityObject>();
                System.Type componentType = GetType(componentTypeName, assemblyName);
                if (componentType != null)
                {
                    foreach (UnityEngine.GameObject testableObject in FindObjectsOfType<UnityEngine.GameObject>())
                    {
                        if (name == "" || testableObject.GetComponent(componentType) != null)
                        {
                            foundObjects.Add(GameObjectToAltUnityObject(testableObject,camera));
                        }
                    }

                    response = Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);
                }
                else
                {
                    response = errorComponentNotFoundMessage;
                }
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }
        });

    }

    private void FindObjectsWhereNameContains(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = System.Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.Camera camera=null;
                if(cameraName!=null){
                    camera = UnityEngine.Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                }
                
                System.Collections.Generic.List<AltUnityObject> foundObjects = new System.Collections.Generic.List<AltUnityObject>();
                foreach (UnityEngine.GameObject testableObject in FindObjectsOfType<UnityEngine.GameObject>())
                {
                    if (testableObject.name.Contains(objectName))
                    {
                        foundObjects.Add(GameObjectToAltUnityObject(testableObject,camera));
                    }
                }

                response = Newtonsoft.Json.JsonConvert.SerializeObject(foundObjects);

            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }

        });

    }

    private void GetAllObjects(string methodParameter, AltClientSocketHandler handler)
    {
        var parameters = ";" + methodParameter;
        FindObjectsByName(parameters, handler);
    }

    private void GetCurrentScene(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            AltUnityObject scene = new AltUnityObject(name: UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                       type: "UnityScene");
            handler.SendResponse(UnityEngine.JsonUtility.ToJson(scene));
        });
    }


    private void ClickEvent(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.GameObject foundGameObject = GetGameObject(altUnityObject);
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.EventSystems.ExecuteEvents.Execute(foundGameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }

        });
    }

    private void ClickOnScreenAtXy(string x, string y, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                MockUpPointerInputModule mockUp = new MockUpPointerInputModule();
                UnityEngine.Touch touch = new UnityEngine.Touch { position = new UnityEngine.Vector2(float.Parse(x), float.Parse(y)), phase = UnityEngine.TouchPhase.Began};
                var pointerEventData = mockUp.ExecuteTouchEvent(touch);
                if (pointerEventData.pointerPress == null &&
                    pointerEventData.pointerEnter == null &&
                    pointerEventData.pointerDrag == null)
                {
                    response = errorNotFoundMessage;
                }
                else
                {
                    UnityEngine.GameObject gameObject = pointerEventData.pointerPress.gameObject;

                    UnityEngine.Debug.Log("GameOBject: " + gameObject);

                    gameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
                    gameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                    gameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
                    UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                    gameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                    gameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
                    UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                    gameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);
                    touch.phase = UnityEngine.TouchPhase.Ended;
                    mockUp.ExecuteTouchEvent(touch, pointerEventData);

                    response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject, pointerEventData.enterEventCamera));
                }

            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }

        });
    }

    private void GetObjectComponentProperty(string altObjectString, string propertyString, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorPropertyNotFoundMessage;
            try
            {
                AltUnityObjectProperty altProperty = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
                AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                UnityEngine.GameObject testableObject = GetGameObject(altUnityObject);
                System.Reflection.MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
                response = GetValueForMember(memberInfo, testableObject, altProperty);
            }
            catch (Newtonsoft.Json.JsonException e)
            {
                UnityEngine.Debug.Log(e);
                if (altObjectString.Contains("error"))
                {
                    response = errorObjectWasNotFound;
                }
                else
                {
                    response = errorCouldNotParseJsonString;
                }
            }
            catch (System.NullReferenceException e)
            {
                UnityEngine.Debug.Log(e);
                response = errorComponentNotFoundMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }
            handler.SendResponse(response);
        });
    }

    private void SetObjectComponentProperty(string altObjectString, string propertyString, string valueString, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorPropertyNotFoundMessage;
            try
            {
                AltUnityObjectProperty altProperty =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
                AltUnityObject altUnityObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                UnityEngine.GameObject testableObject = GetGameObject(altUnityObject);
                System.Reflection.MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
                response = SetValueForMember(memberInfo, valueString, testableObject, altProperty);
            }
            catch (Newtonsoft.Json.JsonException e)
            {
                UnityEngine.Debug.Log(e);
                if (altObjectString.Contains("error"))
                {
                    response = errorObjectWasNotFound;
                }
                else
                {
                    response = errorCouldNotParseJsonString;
                }
            }
            catch (System.NullReferenceException e)
            {
                UnityEngine.Debug.Log(e);
                response = errorComponentNotFoundMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }
            handler.SendResponse(response);
        });
    }

    private void CallComponentMethodForObject(string altObjectString, string actionString, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorMethodNotFoundMessage;
            try
            {
                System.Reflection.MethodInfo methodInfoToBeInvoked;
                AltUnityObjectAction altAction = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObjectAction>(actionString);
                var componentType = GetType(altAction.Component, altAction.Assembly);

                System.Reflection.MethodInfo[] methodInfos = GetMethodInfoWithSpecificName(componentType, altAction.Method);
                if (methodInfos.Length == 1)
                    methodInfoToBeInvoked = methodInfos[0];
                else
                {
                    methodInfoToBeInvoked = GetMethodToBeInvoked(methodInfos, altAction);
                }



                if (string.IsNullOrEmpty(altObjectString))
                {
                    response = InvokeMethod(methodInfoToBeInvoked, altAction, null, response);
                }
                else
                {
                    AltUnityObject altObject = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                    UnityEngine.GameObject gameObject = GetGameObject(altObject);
                    if (componentType == typeof(UnityEngine.GameObject))
                    {
                        response = InvokeMethod(methodInfoToBeInvoked, altAction, gameObject, response);
                    }
                    else
                    if (gameObject.GetComponent(componentType) != null)
                    {
                        UnityEngine.Component component = gameObject.GetComponent(componentType);
                        response = InvokeMethod(methodInfoToBeInvoked, altAction, component, response);
                    }
                }
            }
            catch (System.ArgumentException)
            {
                response = errorFailedToParseArguments;
            }
            catch (System.Reflection.TargetParameterCountException)
            {
                response = errorIncorrectNumberOfParameters;
            }
            catch (Newtonsoft.Json.JsonException e)
            {
                UnityEngine.Debug.Log(e);
                response = altObjectString.Contains("error") ? errorObjectWasNotFound : errorCouldNotParseJsonString;
            }
            catch (System.NullReferenceException)
            {
                response = errorComponentNotFoundMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }
            handler.SendResponse(response);
        });
    }

    private System.Reflection.MethodInfo[] GetMethodInfoWithSpecificName(System.Type componentType, string altActionMethod)
    {
        System.Reflection.MethodInfo[] methodInfos = componentType.GetMethods();
        return methodInfos.Where(method => method.Name.Equals(altActionMethod)).ToArray();
    }

    private System.Reflection.MethodInfo GetMethodToBeInvoked(System.Reflection.MethodInfo[] methodInfos, AltUnityObjectAction altUnityObjectAction)
    {
        var parameter = altUnityObjectAction.Parameters.Split('?');
        var typeOfParametes = altUnityObjectAction.TypeOfParameters.Split('?');
        methodInfos = methodInfos.Where(method => method.GetParameters().Length == parameter.Length).ToArray();
        if (methodInfos.Length == 1)
            return methodInfos[0];
        foreach (var methodInfo in methodInfos)
        {
            try
            {
                for (int counter = 0; counter < typeOfParametes.Length; counter++)
                {
                    System.Type type = System.Type.GetType(typeOfParametes[counter]);
                    if (methodInfo.GetParameters()[counter].ParameterType != type)
                        throw new System.Exception("Missmatch in parameter type");
                }
                //If every parameter can be deserialize then this is our method(except if there int but method can take also int)
                return methodInfo;

            }
            catch (System.Exception e)
            {

            }

        }

        var errorMessage = "No method found with this signature: " + altUnityObjectAction.Method + "(";
        errorMessage = typeOfParametes.Aggregate(errorMessage, (current, typeOfParamete) => current + (typeOfParamete + ","));

        errorMessage = errorMessage.Remove(errorMessage.Length - 1);
        errorMessage += ")";
        throw new System.Exception(errorMessage);
    }

    private static string InvokeMethod(System.Reflection.MethodInfo methodInfo, AltUnityObjectAction altAction, object component, string response)
    {
        if (methodInfo == null) return response;
        if (altAction.Parameters == "")
        {
            response = Newtonsoft.Json.JsonConvert.SerializeObject(methodInfo.Invoke(component, null));
        }
        else
        {
            System.Reflection.ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            string[] parameterStrings = altAction.Parameters.Split('?');
            if (parameterInfos.Length != parameterStrings.Length)
                throw new System.Reflection.TargetParameterCountException();
            object[] parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (parameterInfos[i].ParameterType == typeof(string))
                    parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(parameterStrings[i]),
                        parameterInfos[i].ParameterType);
                else
                {
                    parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(parameterStrings[i], parameterInfos[i].ParameterType);
                }
            }

            response = Newtonsoft.Json.JsonConvert.SerializeObject(methodInfo.Invoke(component, parameters));
        }
        return response;
    }

    private void CloseConnection(AltClientSocketHandler handler)
    {
        UnityEngine.Debug.Log("Close connection event handler!");
        _socketServer.StartListeningForConnections();

    }

    private void UnknownString(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            handler.SendResponse(errorCouldNotPerformOperationMessage);
        });
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

    public static System.Type GetType(string typeName, string assemblyName)
    {
        var type = System.Type.GetType(typeName);

        if (type != null)
            return type;
        if (assemblyName == null || assemblyName.Equals(""))
        {
            if (typeName.Contains("."))
            {
                assemblyName = typeName.Substring(0, typeName.LastIndexOf('.'));
                UnityEngine.Debug.Log("assembly name " + assemblyName);
                try
                {
                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly.GetType(typeName) == null)
                        return null;
                    return assembly.GetType(typeName);
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e);
                    return null;
                }
            }

            return null;
        }
        else
        {
            try
            {
                var assembly = System.Reflection.Assembly.Load(assemblyName);
                if (assembly.GetType(typeName) == null)
                    return null;
                return assembly.GetType(typeName);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log(e);
                return null;
            }

        }
    }

    private static UnityEngine.GameObject GetGameObject(AltUnityObject altUnityObject)
    {
        foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
        {
            if (gameObject.GetInstanceID() == altUnityObject.id)
                return gameObject;
        }
        return null;
    }
    private static UnityEngine.GameObject GetGameObject(int objectId)
    {
        foreach (UnityEngine.GameObject gameObject in FindObjectsOfType<UnityEngine.GameObject>())
        {
            if (gameObject.GetInstanceID() == objectId)
                return gameObject;
        }
        return null;
    }

    private System.Reflection.MemberInfo GetMemberForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectProperty altUnityObjectProperty)
    {
        System.Reflection.MemberInfo memberInfo = null;
        System.Type componentType = null;
        componentType = GetType(altUnityObjectProperty.Component, altUnityObjectProperty.Assembly);
        System.Reflection.PropertyInfo propertyInfo = componentType.GetProperty(altUnityObjectProperty.Property);
        System.Reflection.FieldInfo fieldInfo = componentType.GetField(altUnityObjectProperty.Property);
        if (GetGameObject(altUnityObject).GetComponent(componentType) != null)
        {
            if (propertyInfo != null)
                return propertyInfo;
            if (fieldInfo != null)
                return fieldInfo;
        }
        return memberInfo;
    }


    private System.Reflection.MethodInfo GetMethodForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectAction altUnityObjectAction)
    {
        System.Type componentType = null;
        componentType = GetType(altUnityObjectAction.Component, altUnityObjectAction.Assembly);
        System.Reflection.MethodInfo methodInfo = componentType.GetMethod(altUnityObjectAction.Method);
        return methodInfo;
    }

    private string GetValueForMember(System.Reflection.MemberInfo memberInfo, UnityEngine.GameObject testableObject, AltUnityObjectProperty altProperty)
    {
        string response = errorPropertyNotFoundMessage;
        if (memberInfo != null)
        {
            if (memberInfo.MemberType == System.Reflection.MemberTypes.Property)
            {
                System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)memberInfo;
                object value = propertyInfo.GetValue(testableObject.GetComponent(GetType(altProperty.Component, altProperty.Assembly)), null);
                response = SerializeMemberValue(value, propertyInfo.PropertyType);
            }
            if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
            {
                System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
                object value = fieldInfo.GetValue(testableObject.GetComponent(GetType(altProperty.Component, altProperty.Assembly)));
                response = SerializeMemberValue(value, fieldInfo.FieldType);
            }
        }
        return response;
    }

    private string SetValueForMember(System.Reflection.MemberInfo memberInfo, string valueString, UnityEngine.GameObject testableObject, AltUnityObjectProperty altProperty)
    {
        string response = errorPropertyNotFoundMessage;
        if (memberInfo != null)
        {
            if (memberInfo.MemberType == System.Reflection.MemberTypes.Property)
            {
                System.Reflection.PropertyInfo propertyInfo = (System.Reflection.PropertyInfo)memberInfo;
                try
                {
                    object value = DeserializeMemberValue(valueString, propertyInfo.PropertyType);
                    if (value != null)
                    {
                        propertyInfo.SetValue(testableObject.GetComponent(altProperty.Component), value, null);
                        response = "valueSet";
                    }
                    else
                        response = errorPropertyNotSet;
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e);
                    response = errorPropertyNotSet;
                }
            }
            if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
            {
                System.Reflection.FieldInfo fieldInfo = (System.Reflection.FieldInfo)memberInfo;
                try
                {
                    object value = DeserializeMemberValue(valueString, fieldInfo.FieldType);
                    if (value != null)
                    {
                        fieldInfo.SetValue(testableObject.GetComponent(altProperty.Component), value);
                        response = "valueSet";
                    }
                    else
                        response = errorPropertyNotSet;
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e);
                    response = errorPropertyNotSet;
                }
            }
        }
        return response;
    }

    private string SerializeMemberValue(object value, System.Type type)
    {
        string response;
        if (type == typeof(string))
            return value.ToString();
        try
        {
            response = Newtonsoft.Json.JsonConvert.SerializeObject(value, type, _jsonSettings);
        }
        catch (Newtonsoft.Json.JsonException)
        {
            response = value.ToString();
        }
        return response;
    }

    private object DeserializeMemberValue(string valueString, System.Type type)
    {
        object value;
        if (type == typeof(string))
            valueString = Newtonsoft.Json.JsonConvert.SerializeObject(valueString);
        try
        {
            value = Newtonsoft.Json.JsonConvert.DeserializeObject(valueString, type);
        }
        catch (Newtonsoft.Json.JsonException)
        {
            value = null;
        }
        return value;
    }
#if ALTUNITYTESTER

    private void SetMovingTouch(UnityEngine.Vector2 start, UnityEngine.Vector2 destination, string duration, AltClientSocketHandler handler)
    {

        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.Touch touch = new UnityEngine.Touch();
                touch.phase = UnityEngine.TouchPhase.Began;
                touch.position = start;
                System.Collections.Generic.List<UnityEngine.Touch> touches = Input.touches.ToList();
                touches.Sort((touch1, touch2) => (touch1.fingerId.CompareTo(touch2.fingerId)));
                int fingerId = 0;
                foreach (UnityEngine.Touch iter in touches)
                {
                    if (iter.fingerId != fingerId)
                        break;
                    fingerId++;
                }

                touch.fingerId = fingerId;
                Input.SetMovingTouch(touch, destination, float.Parse(duration));
                response = "Ok";
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });

    }
    private void HoldButton(UnityEngine.KeyCode keyCode,float power, float duration, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var powerClamped = UnityEngine.Mathf.Clamp01(power);
            Input.SetKeyDown(keyCode,power, duration);
            handler.SendResponse("Ok");
        });
    }
    private void MoveMouse(UnityEngine.Vector2 location, float duration, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            Input.MoveMouse(location, duration);
            handler.SendResponse("Ok");
        });
    }
    private void ScrollMouse(float scrollValue, float duration, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            Input.Scroll(scrollValue, duration);
            handler.SendResponse("Ok");
        });
    }
#endif
    private void DragObject(UnityEngine.Vector2 position, AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                MockUpPointerInputModule mockUp = new MockUpPointerInputModule();
                var pointerEventData = mockUp.ExecuteTouchEvent(new UnityEngine.Touch() { position = position });
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Camera viewingCamera = FoundCameraById(altUnityObject.idCamera);
                UnityEngine.Vector3 gameObjectPosition = viewingCamera.WorldToScreenPoint(gameObject.transform.position);
                pointerEventData.delta = pointerEventData.position - new UnityEngine.Vector2(gameObjectPosition.x, gameObjectPosition.y);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dragHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void DropObject(UnityEngine.Vector2 position, AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.dropHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void PointerUpFromObject(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void PointerDownFromObject(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                if (camera != null)
                {
                    response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject, camera));
                }
                else
                {

                    response = Newtonsoft.Json.JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject));
                }
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void PointerEnterObject(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void PointerExitObject(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);
                UnityEngine.EventSystems.ExecuteEvents.Execute(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
#if ALTUNITYTESTER
    private void Tilt(UnityEngine.Vector3 acceleration, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                Input.acceleration = acceleration;
                response = "OK";
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
#endif

    private void LoadScene(string scene, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {

                UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
                response = "Ok";

            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }
        });
    }

    //    private bool IsSceneInBuild(string scene)
    //    {
    //        UnityEngine.Debug.Log("Scenesnumber:"+UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
    //        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
    //        {
    //            UnityEngine.Debug.Log(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(i));
    //            if (UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(i).name.Equals(scene))
    //                return true;
    //        }
    //
    //        return false;
    //    }

    private UnityEngine.Camera FoundCameraById(int id)
    {
        foreach (var camera in UnityEngine.Camera.allCameras)
        {
            if (camera.GetInstanceID() == id)
                return camera;
        }

        return null;
    }

    private void DeletePlayerPref(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.PlayerPrefs.DeleteAll();
                response = "Ok";
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }

    private void DeleteKeyPlayerPref(string keyName, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                UnityEngine.PlayerPrefs.DeleteKey(keyName);
                response = "Ok";
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }

    private void SetKeyPlayerPref(string keyName, string valueName, PLayerPrefKeyType option, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                switch (option)
                {
                    case PLayerPrefKeyType.String:
                        UnityEngine.Debug.Log("Set Option string ");
                        UnityEngine.PlayerPrefs.SetString(keyName, valueName);
                        break;
                    case PLayerPrefKeyType.Float:
                        UnityEngine.Debug.Log("Set Option Float ");
                        UnityEngine.PlayerPrefs.SetFloat(keyName, float.Parse(valueName));
                        break;
                    case PLayerPrefKeyType.Int:
                        UnityEngine.Debug.Log("Set Option Int ");
                        UnityEngine.PlayerPrefs.SetInt(keyName, int.Parse(valueName));
                        break;
                }

                response = "Ok";
            }
            catch (System.FormatException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorFormatException;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void GetKeyPlayerPref(string keyName, PLayerPrefKeyType option, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                if (UnityEngine.PlayerPrefs.HasKey(keyName))
                {
                    switch (option)
                    {
                        case PLayerPrefKeyType.String:
                            UnityEngine.Debug.Log("Option string " + UnityEngine.PlayerPrefs.GetString(keyName));
                            response = UnityEngine.PlayerPrefs.GetString(keyName);
                            break;
                        case PLayerPrefKeyType.Float:
                            UnityEngine.Debug.Log("Option Float " + UnityEngine.PlayerPrefs.GetFloat(keyName));
                            response = UnityEngine.PlayerPrefs.GetFloat(keyName) + "";
                            break;
                        case PLayerPrefKeyType.Int:
                            UnityEngine.Debug.Log("Option Int " + UnityEngine.PlayerPrefs.GetInt(keyName));
                            response = UnityEngine.PlayerPrefs.GetInt(keyName) + "";
                            break;
                    }
                }
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
#if ALTUNITYTESTER
    private void ActionFinished(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                if(Input.Finished)
                    response = "Yes";
                else
                {
                    response = "No";
                }
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
#endif

    private void Tap(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
                UnityEngine.GameObject gameObject = GetGameObject(altUnityObject);
                UnityEngine.Debug.Log("GameOBject: " + gameObject);

                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
                gameObject.SendMessage("OnMouseEnter", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerDownHandler);
                gameObject.SendMessage("OnMouseDown", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.initializePotentialDrag);
                gameObject.SendMessage("OnMouseOver", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerUpHandler);
                gameObject.SendMessage("OnMouseUp", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);
                gameObject.SendMessage("OnMouseUpAsButton", UnityEngine.SendMessageOptions.DontRequireReceiver);
                UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                gameObject.SendMessage("OnMouseExit", UnityEngine.SendMessageOptions.DontRequireReceiver);

                var camera = FoundCameraById(altUnityObject.idCamera);
                response = Newtonsoft.Json.JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (System.NullReferenceException exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }

    private void GetAllComponents(string ObjectId, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            UnityEngine.GameObject altObject = GetGameObject(System.Convert.ToInt32(ObjectId));
            System.Collections.Generic.List<AltUnityComponent> listComponents = new System.Collections.Generic.List<AltUnityComponent>();
            foreach (var component in altObject.GetComponents<UnityEngine.Component>())
            {
                var a = component.GetType();
                var componentName = a.FullName;
                var assemblyName = a.Assembly.GetName().Name;
                listComponents.Add(new AltUnityComponent(componentName, assemblyName));
            }

            var response = Newtonsoft.Json.JsonConvert.SerializeObject(listComponents);
            handler.SendResponse(response);
        });
    }
    private void GetAllFields(string id, AltUnityComponent component, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            UnityEngine.GameObject altObject;
            altObject = id.Equals("null") ? null : GetGameObject(System.Convert.ToInt32(id));
            System.Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var fieldInfos = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            System.Collections.Generic.List<AltUnityField> listFields = new System.Collections.Generic.List<AltUnityField>();

            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    listFields.Add(new AltUnityField(fieldInfo.Name,
                        value == null ? "null" : value.ToString()));
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e.StackTrace);
                }
            }
            handler.SendResponse(Newtonsoft.Json.JsonConvert.SerializeObject(listFields));
        });
    }

    private void GetAllMethods(AltUnityComponent component, AltClientSocketHandler handler)
    {

        _responseQueue.ScheduleResponse(delegate
        {
            System.Type type = GetType(component.componentName, component.assemblyName);
            var methodInfos = type.GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            System.Collections.Generic.List<string> listMethods = new System.Collections.Generic.List<string>();

            foreach (var methodInfo in methodInfos)
            {
                listMethods.Add(methodInfo.ToString());
            }
            handler.SendResponse(Newtonsoft.Json.JsonConvert.SerializeObject(listMethods));
        });
    }
    private void GetAllScenes(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            System.Collections.Generic.List<string> SceneNames = new System.Collections.Generic.List<string>();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                var s = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
                SceneNames.Add(s);
            }
            handler.SendResponse(Newtonsoft.Json.JsonConvert.SerializeObject(SceneNames));
        });

    }
    private void GetAllCameras(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var cameras = FindObjectsOfType<UnityEngine.Camera>();
            System.Collections.Generic.List<string> cameraNames = new System.Collections.Generic.List<string>();
            foreach (UnityEngine.Camera camera in cameras)
            {
                cameraNames.Add(camera.name);
            }
            handler.SendResponse(Newtonsoft.Json.JsonConvert.SerializeObject(cameraNames));
        });
    }

    private void SetTimeScale(float timeScale, AltClientSocketHandler handler) {
        _responseQueue.ScheduleResponse(delegate {
            string response = errorCouldNotPerformOperationMessage;
            try {
                UnityEngine.Time.timeScale = timeScale;
                response = "Ok";
            } catch (System.Exception exception) {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            } finally {
                handler.SendResponse(response);

            }
        });
    }

    private void GetTimeScale(AltClientSocketHandler handler) {
        _responseQueue.ScheduleResponse(delegate {
            string response = errorCouldNotPerformOperationMessage;
            try {
                response = Newtonsoft.Json.JsonConvert.SerializeObject(UnityEngine.Time.timeScale);
            } catch (System.Exception exception) {
                UnityEngine.Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            } finally {
                handler.SendResponse(response);

            }
        });
    }

    

    private void HightObjectFromCoordinates(UnityEngine.Vector2 screenCoordinates, string ColorAndWidth, UnityEngine.Vector2 size, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, System.StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, System.StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]); 

            UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);

            UnityEngine.Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenCoordinates);
            UnityEngine.RaycastHit[] hits;
            var raycasters = FindObjectsOfType<UnityEngine.UI.GraphicRaycaster>();
            UnityEngine.EventSystems.PointerEventData pointerEventData = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            pointerEventData.position = screenCoordinates;
            foreach (var raycaster in raycasters)
            {
                System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> hitUI = new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
                raycaster.Raycast(pointerEventData, hitUI);
                foreach (var hit in hitUI)
                {
                    StartCoroutine(HighLightSelectedObjectCorutine(hit.gameObject, color, width, size, handler));
                    return;
                }
            }
            hits = UnityEngine.Physics.RaycastAll(ray);
            if (hits.Length > 0)
            {
                StartCoroutine(HighLightSelectedObjectCorutine(hits[hits.Length - 1].transform.gameObject, color, width, size, handler));
            }
            else
            {
                GetScreenshot(size, handler);
            }
        });
    }
    private void HighLightSelectedObject(int id, string ColorAndWidth, UnityEngine.Vector2 size, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, System.StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, System.StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);

            UnityEngine.Color color = new UnityEngine.Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);
            var gameObject = GetGameObject(id);

            if (gameObject != null)
            {
                StartCoroutine(HighLightSelectedObjectCorutine(gameObject, color, width, size, handler));
            }
            else
                GetScreenshot(size, handler);
        });
    }
    System.Collections.IEnumerator HighLightSelectedObjectCorutine(UnityEngine.GameObject gameObject, UnityEngine.Color color, float width, UnityEngine.Vector2 size, AltClientSocketHandler handler)
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
            GetScreenshot(size, handler);
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
                GetScreenshot(size, handler);
                while (!destroyHightlight)
                    yield return null;
                Destroy(panelHighlight);
                destroyHightlight = false;
            }
            else
            {
                GetScreenshot(size, handler);
            }
        }

    }
    private void GetScreenshot(UnityEngine.Vector2 size, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate {
            StartCoroutine(TakeScreenshot(size, handler));
        });
        
    }

    private void ScreenshotReady(UnityEngine.Texture2D screenshot, UnityEngine.Vector2 size, AltClientSocketHandler handler) {
        _responseQueue.ScheduleResponse(delegate {
            int width = (int)size.x;
            int height = (int)size.y;

            var heightDifference = screenshot.height - height;
            var widthDifference = screenshot.width - width;
            if (heightDifference >= 0 || widthDifference >= 0)
            {
                if (heightDifference > widthDifference)
                {
                    width = height * screenshot.width / screenshot.height;
                }
                else
                {
                    height = width * screenshot.height / screenshot.width;
                }
            }
            string[] fullResponse = new string[5];

            fullResponse[0] = Newtonsoft.Json.JsonConvert.SerializeObject(new UnityEngine.Vector2(screenshot.width, screenshot.height), new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            TextureScale.Bilinear(screenshot, width, height);
            screenshot.Apply(true);
            screenshot.Compress(false);
            screenshot.Apply(false);


            var screenshotSerialized = screenshot.GetRawTextureData();
            UnityEngine.Debug.Log(screenshotSerialized.LongLength + " size after Unity Compression");
            UnityEngine.Debug.Log(System.DateTime.Now + " Start Compression");
            var screenshotCompressed = CompressScreenshot(screenshotSerialized);
            UnityEngine.Debug.Log(System.DateTime.Now + " Finished Compression");
            var length = screenshotCompressed.LongLength;
            fullResponse[1] = length.ToString();

            var format = screenshot.format;
            fullResponse[2] = format.ToString();

            var newSize = new UnityEngine.Vector3(screenshot.width, screenshot.height);
            fullResponse[3] = Newtonsoft.Json.JsonConvert.SerializeObject(newSize, new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            UnityEngine.Debug.Log(System.DateTime.Now + " Serialize screenshot");
            fullResponse[4] = Newtonsoft.Json.JsonConvert.SerializeObject(screenshotCompressed, new Newtonsoft.Json.JsonSerializerSettings
            {
                StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeNonAscii
            });
        
            UnityEngine.Debug.Log(System.DateTime.Now + " Finished Serialize Screenshot Start serialize response");
            handler.SendResponse(Newtonsoft.Json.JsonConvert.SerializeObject(fullResponse));
            UnityEngine.Debug.Log(System.DateTime.Now + " Finished send Response");
            Destroy(screenshot);
            destroyHightlight = true;
        });
    }

    private System.Collections.IEnumerator TakeScreenshot(UnityEngine.Vector2 size, AltClientSocketHandler handler) {
        yield return new UnityEngine.WaitForEndOfFrame();
        var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
        AltUnityEvents.Instance.ScreenshotReady.Invoke(screenshot, size, handler);
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
}



