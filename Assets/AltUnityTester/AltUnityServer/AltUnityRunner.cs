using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Reflection;
using Newtonsoft.Json;

public partial class AltUnityRunner : MonoBehaviour, AltIClientSocketHandlerDelegate
{
    private static AltUnityRunner _altUnityRunner;
    private Vector3 _position;
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

    private JsonSerializerSettings _jsonSettings;


    enum FindOption
    {
        Name, ContainName, Component
    }
    public int SocketPortNumber = 13000;
    public bool DebugBuildNeeded = true;

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

        _jsonSettings = new JsonSerializerSettings();
        _jsonSettings.NullValueHandling = NullValueHandling.Ignore;

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
        AltUnityEvents.Instance.SwipeFinished.AddListener(SwipeFinished);
#endif

        AltUnityEvents.Instance.LoadScene.AddListener(LoadScene);
        AltUnityEvents.Instance.SetKeyPlayerPref.AddListener(SetKeyPlayerPref);
        AltUnityEvents.Instance.GetKeyPlayerPref.AddListener(GetKeyPlayerPref);
        AltUnityEvents.Instance.DeleteKeyPlayerPref.AddListener(DeleteKeyPlayerPref);
        AltUnityEvents.Instance.DeletePlayerPref.AddListener(DeletePlayerPref);



        AltUnityEvents.Instance.GetAllComponents.AddListener(GetAllComponents);
        AltUnityEvents.Instance.GetAllMethods.AddListener(GetAllMethods);
        AltUnityEvents.Instance.GetAllProperties.AddListener(GetAllProperties);


        if (DebugBuildNeeded && !Debug.isDebugBuild)
        {
            Debug.Log("AltUnityTester will not run if this is not a Debug/Development build");
        }
        else
        {
            DontDestroyOnLoad(this);
            StartSocketServer();
            Debug.Log("AltUnity Driver started");
        }

    }

  


    /// <summary>
    /// Start listening to client after server starts
    /// </summary>
    public void StartSocketServer()
    {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;
        string separatorString = "&";
        Encoding encoding = Encoding.UTF8;

        _socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, SocketPortNumber, maxClients, separatorString, encoding);

        _socketServer.StartListeningForConnections();

        Debug.Log(String.Format(
            "AltUnity Server at {0} on port {1}",
            _socketServer.LocalEndPoint.Address, _socketServer.PortNumber));
    }

    void OnApplicationQuit()
    {
        CleanUp();
    }

    public void CleanUp()
    {
        Debug.Log("Cleaning up socket server");
        _socketServer.Cleanup();
    }

    /// <summary>
    /// Get screen coordinates for an object
    /// </summary>
    /// <param name="gameObject"> The object wich position is returned</param>
    /// <returns>Screen coordinates for the object</returns>
    private Vector3 getObjectScreePosition(GameObject gameObject, Camera camera)
    {
        Canvas canvasParent = gameObject.GetComponentInParent<Canvas>();
        if (canvasParent != null)
        {
            if (canvasParent.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                Vector3[] vector3S = new Vector3[4];
                gameObject.GetComponent<RectTransform>().GetWorldCorners(vector3S);
                var center = new Vector3((vector3S[0].x + vector3S[2].x) / 2, (vector3S[0].y + vector3S[2].y) / 2, (vector3S[0].z + vector3S[2].z) / 2);
                return canvasParent.worldCamera.WorldToScreenPoint(center);
            }
            if (gameObject.GetComponent<RectTransform>() != null)
            {
                return gameObject.GetComponent<RectTransform>().position;
            }
            return camera.WorldToScreenPoint(gameObject.transform.position);
        }

        if (gameObject.GetComponent<Collider>() != null)
        {
            return camera.WorldToScreenPoint(gameObject.GetComponent<Collider>().bounds.center);
        }

        return camera.WorldToScreenPoint(gameObject.transform.position);
    }


    /// <summary>
    /// Tranforms an GameObejct to AltUnityObject
    /// </summary>
    /// <param name="altGameObject">GameObject that will be transformed</param>
    /// <returns>An AltUnityObject with information about GameObject</returns>
    private AltUnityObject GameObjectToAltUnityObject(GameObject altGameObject, Camera camera = null)
    {
        if (camera == null)//if no camera is given it will iterate through all cameras until found one that can see the object if no camera can see the object it will return the position from the last camera
        {
            foreach (var camera1 in Camera.allCameras)
            {
                _position = getObjectScreePosition(altGameObject, camera1);
                camera = camera1;
                if (_position.x > 0 && _position.y > 0 && _position.x < Screen.width && _position.y < Screen.height && _position.z >= 0)//Check if camera can see the object
                {
                    break;
                }
            }
        }
        else
        {
            _position = getObjectScreePosition(altGameObject, camera);
        }


        AltUnityObject altObject = new AltUnityObject(name: altGameObject.name,
                                                      id: altGameObject.GetInstanceID(),
                                                      x: Convert.ToInt32(Mathf.Round(_position.x)),
                                                      y: Convert.ToInt32(Mathf.Round(_position.y)),
                                                      z: Convert.ToInt32(Mathf.Round(_position.z)),//if z is negative that means the cannot see the object(object is behind the camera)
                                                      mobileY: Convert.ToInt32(Mathf.Round(Screen.height - _position.y)),
                                                      type: "",
                                                      enabled: altGameObject.activeSelf,
                                                      worldX: _position.x,
                                                      worldY: _position.y,
                                                      worldZ: _position.z,
                                                      idCamera: camera.GetInstanceID());
        return altObject;
    }


    public void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message)
    {
        string[] separator = new string[] { ";" };
        string[] pieces = message.Split(separator, StringSplitOptions.None);
        AltUnityComponent altComponent;
        AltUnityObject altUnityObject;
        string methodParameters;
        PLayerPrefKeyType option;
        switch (pieces[0])
        {
            case "findAllObjects":
                Debug.Log("all objects requested");
                methodParameters = pieces[1] + ";" + pieces[2];
                AltUnityEvents.Instance.GetAllObjects.Invoke(methodParameters, handler);
                break;
            case "findObjectByName":
                Debug.Log("find object by name " + pieces[1]);
                Debug.Log(pieces.Length);
                methodParameters = pieces[1] + ";" + pieces[2] + ";" + pieces[3];
                AltUnityEvents.Instance.FindObjectByName.Invoke(methodParameters, handler);
                break;
            case "findObjectWhereNameContains":
                Debug.Log("find object where name contains:" + pieces[1]);
                methodParameters = pieces[1] + ";" + pieces[2] + ";" + pieces[3];
                AltUnityEvents.Instance.FindObjectWhereNameContains.Invoke(methodParameters, handler);
                break;
            case "tapObject":
                try
                {
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    Debug.Log("tapped object by name " + altUnityObject.name);
                    AltUnityEvents.Instance.Tap.Invoke(altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "findObjectsByName":
                Debug.Log("find multiple objects by name " + pieces[1]);
                methodParameters = pieces[1] + ";" + pieces[2] + ";" + pieces[3];
                AltUnityEvents.Instance.FindObjectsByName.Invoke(methodParameters, handler);
                break;
            case "findObjectsWhereNameContains":
                Debug.Log("find objects where name contains:" + pieces[1]);
                methodParameters = pieces[1] + ";" + pieces[2] + ";" + pieces[3];
                AltUnityEvents.Instance.FindObjectsWhereNameContains.Invoke(methodParameters, handler);
                break;
            case "getCurrentScene":
                Debug.Log("get current scene");
                AltUnityEvents.Instance.GetCurrentScene.Invoke(handler);
                break;
            case "findObjectByComponent":
                Debug.Log("find object by component " + pieces[1]);
                methodParameters = pieces[1] + ";" + pieces[2] + ";" + pieces[3] + ";" + pieces[4];
                AltUnityEvents.Instance.FindObjectByComponent.Invoke(methodParameters, handler);
                break;
            case "findObjectsByComponent":
                Debug.Log("find objects by component " + pieces[1]);
                methodParameters = pieces[1] + ";" + pieces[2] + ";" + pieces[3] + ";" + pieces[4];
                AltUnityEvents.Instance.FindObjectsByComponent.Invoke(methodParameters, handler);
                break;
            case "getObjectComponentProperty":
                Debug.Log("get property " + pieces[2] + " for object " + pieces[1]);
                AltUnityEvents.Instance.GetComponentProperty.Invoke(pieces[1], pieces[2], handler);
                break;
            case "setObjectComponentProperty":
                Debug.Log("set property " + pieces[2] + " to value: " + pieces[3] + " for object " + pieces[1]);
                AltUnityEvents.Instance.SetComponentProperty.Invoke(pieces[1], pieces[2], pieces[3], handler);
                break;
            case "callComponentMethodForObject":
                Debug.Log("call action " + pieces[2] + " for object " + pieces[1]);
                AltUnityEvents.Instance.CallComponentMethod.Invoke(pieces[1], pieces[2], handler);
                break;
            case "closeConnection":
                Debug.Log("Socket connection closed!");
                AltUnityEvents.Instance.CloseConnection.Invoke(handler);
                break;
            case "clickEvent":
                Debug.Log("ClickEvent on " + pieces[1]);
                try
                {
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.ClickEvent.Invoke(altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }

                break;
            case "tapScreen":
                Debug.Log("Screen tapped at X:" + pieces[1] + " Y:" + pieces[2]);
                AltUnityEvents.Instance.TapScreen.Invoke(pieces[1], pieces[2], handler);
                break;
            case "dragObject":
                try
                {
                    Debug.Log("Drag object");
                    Vector2 positionVector2 = JsonConvert.DeserializeObject<Vector2>(pieces[1]);
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[2]);
                    AltUnityEvents.Instance.DragObject.Invoke(positionVector2, altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "dropObject":
                try
                {
                    Debug.Log("Drop object");
                    Vector2 positionDropVector2 = JsonConvert.DeserializeObject<Vector2>(pieces[1]);
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[2]);
                    AltUnityEvents.Instance.DropObject.Invoke(positionDropVector2, altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "pointerUpFromObject":
                try
                {
                    Debug.Log("PointerUp");
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerUp.Invoke(altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "pointerDownFromObject":
                try
                {
                    Debug.Log("PointerDown");
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerDown.Invoke(altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;

            case "pointerEnterObject":
                try
                {
                    Debug.Log("PointerEnter");
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerEnter.Invoke(altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "pointerExitObject":
                try
                {
                    Debug.Log("PointerExit");
                    altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(pieces[1]);
                    AltUnityEvents.Instance.PointerExit.Invoke(altUnityObject, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;

            case "tilt":
                try
                {
                    Debug.Log("Tilt");
                    Vector3 vector3 = JsonConvert.DeserializeObject<Vector3>(pieces[1]);
                    AltUnityEvents.Instance.Tilt.Invoke(vector3, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;


            case "movingTouch":
                try
                {
                    Debug.Log("Touch");
                    Vector2 start2 = JsonConvert.DeserializeObject<Vector2>(pieces[1]);
                    Vector2 end2 = JsonConvert.DeserializeObject<Vector2>(pieces[2]);
                    AltUnityEvents.Instance.SetMovingTouch.Invoke(start2, end2, pieces[3], handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "loadScene":
                Debug.Log("LoadScene");
                AltUnityEvents.Instance.LoadScene.Invoke(pieces[1], handler);
                break;
            case "deletePlayerPref":
                Debug.Log("deletePlayerPref");
                AltUnityEvents.Instance.DeletePlayerPref.Invoke(handler);
                break;
            case "deleteKeyPlayerPref":
                Debug.Log("deleteKeyPlayerPref");
                AltUnityEvents.Instance.DeleteKeyPlayerPref.Invoke(pieces[1], handler);
                break;
            case "setKeyPlayerPref":
                try
                {
                    Debug.Log("setKeyPlayerPref");
                    option = (PLayerPrefKeyType)Enum.Parse(typeof(PLayerPrefKeyType), pieces[3]);
                    AltUnityEvents.Instance.SetKeyPlayerPref.Invoke(pieces[1], pieces[2], option, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "getKeyPlayerPref":
                try
                {
                    Debug.Log("getKeyPlayerPref");
                    option = (PLayerPrefKeyType)Enum.Parse(typeof(PLayerPrefKeyType), pieces[2]);
                    AltUnityEvents.Instance.GetKeyPlayerPref.Invoke(pieces[1], option, handler);
                }
                catch (JsonException exception)
                {
                    Debug.Log(exception);
                    handler.SendResponse(errorCouldNotParseJsonString);
                }
                break;
            case "swipeFinished":
                Debug.Log("SwipeFinished");
                AltUnityEvents.Instance.SwipeFinished.Invoke(handler);
                break;
            case "getAllComponents":
                Debug.Log("GetAllComponents");
                AltUnityEvents.Instance.GetAllComponents.Invoke(pieces[1],handler);
                break;
            case "getAllProperties":
                Debug.Log("getAllProperties");
                altComponent = JsonConvert.DeserializeObject<AltUnityComponent>(pieces[2]);
                AltUnityEvents.Instance.GetAllProperties.Invoke(pieces[1], altComponent, handler);
                break;
            case "getAllMethods":
                Debug.Log("getAllMethods");
                altComponent = JsonConvert.DeserializeObject<AltUnityComponent>(pieces[1]);
                AltUnityEvents.Instance.GetAllMethods.Invoke(altComponent, handler);
                break;
            default:
                AltUnityEvents.Instance.UnknownString.Invoke(handler);
                break;
        }
    }
    public static GameObject[] GetDontDestroyOnLoadObjects()
{
    GameObject temp = null;
    try
    {
        temp = new GameObject();
        DontDestroyOnLoad( temp );
        Scene dontDestroyOnLoad = temp.scene;
        DestroyImmediate( temp );
        temp = null;
 
        return dontDestroyOnLoad.GetRootGameObjects();
    }
    finally
    {
        if( temp != null )
            DestroyImmediate( temp );
    }
}
    private GameObject FindObjectInScene(string objectName, bool enabled, FindOption option, Type component = null)
    {
        string[] pathList = objectName.Split('/');
        List<int> optionList = new List<int>();
        GameObject foundGameObject = null;
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.transform.parent != null) continue;
            foundGameObject = CheckChildren(obj, pathList, optionList, enabled, option, component);
            if (foundGameObject != null)
            {
                return foundGameObject;
            }
        }
        foreach (var destroyOnLoadObject in GetDontDestroyOnLoadObjects())
        {
            if (destroyOnLoadObject.transform.parent != null) continue;
            foundGameObject = CheckChildren(destroyOnLoadObject, pathList, optionList, enabled, option, component);
            if (foundGameObject != null)
            {
                break;
            }
        }

        return foundGameObject;
    }

    private GameObject CheckChildren(GameObject obj, string[] pathList, List<int> optionList, bool enabled, FindOption option, Type componentType = null)
    {
        if (enabled && obj.activeInHierarchy==false) return null;
        List<int> newOptionList = new List<int>();
        foreach (var i in optionList)
        {
            switch (option)
            {
                case FindOption.Name:
                    if (pathList[i].Equals(obj.name))
                    {
                        if (i + 1 == pathList.Length)
                        {
                            return obj;
                        }
                        else
                        {
                            newOptionList.Add(i + 1);
                        }
                    }
                    break;
                case FindOption.ContainName:
                    if (obj.name.Contains(pathList[i]))
                    {
                        if (i + 1 == pathList.Length)
                        {
                            return obj;
                        }
                        else
                        {
                            newOptionList.Add(i + 1);
                        }
                    }
                    break;
                case FindOption.Component://For finding object that contains object we don't need to search in hierarchy
                    break;
            }

        }

        optionList = newOptionList;
       
        switch (option)
        {
            case FindOption.Name:
                if (obj.name.Equals(pathList[0]))
                {
                    if (pathList.Length == 1)
                    {
                        return obj;
                    }
                    else
                    {
                        optionList.Add(1);

                    }
                }
                break;
            case FindOption.ContainName:
                if (obj.name.Contains(pathList[0]))
                {
                    if (pathList.Length == 1)
                    {
                        return obj;
                    }
                    else
                    {
                        optionList.Add(1);

                    }
                }
                break;
            case FindOption.Component:
                if (obj.GetComponent(componentType) != null)
                {
                    return obj;
                }
                break;
        }


        GameObject gameObject;
        foreach (Transform child in obj.transform)
        {
            gameObject = CheckChildren(child.gameObject, pathList, optionList, enabled, option, componentType);
            if (gameObject != null)
            {
                return gameObject;
            }
        }

        return null;
    }

    private List<GameObject> FindObjectsInScene(string objectName, bool enabled, FindOption option, Type componentType = null)
    {
        string[] pathList = objectName.Split('/');
        List<int> optionList = new List<int>();
        List<GameObject> foundGameObjects = new List<GameObject>();
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.transform.parent != null) continue;
            foundGameObjects.AddRange(CheckChildrenForMultipleGameObjects(obj, pathList, optionList,enabled,option,componentType));
        }
        foreach (var destroyOnLoadObject in GetDontDestroyOnLoadObjects())
        {
            if (destroyOnLoadObject.transform.parent != null) continue;
            foundGameObjects.AddRange(CheckChildrenForMultipleGameObjects(destroyOnLoadObject, pathList, optionList, enabled, option, componentType));
        }

        return foundGameObjects;
    }

    private List<GameObject> CheckChildrenForMultipleGameObjects(GameObject obj, string[] pathList, List<int> optionList,bool enabled, FindOption option, Type componentType = null)
    {
       
            GameObject gameObject;
            List<GameObject> objects = new List<GameObject>();
        if (!(enabled && !obj.activeInHierarchy))
        {
            List<int> newOptionList = new List<int>();
            foreach (var i in optionList)
            {

                switch (option)
                {
                    case FindOption.Name:
                        if (pathList[i].Equals(obj.name))
                        {
                            if (i + 1 == pathList.Length)
                            {
                                objects.Add(obj);
                            }
                            else
                            {
                                newOptionList.Add(i + 1);
                            }
                        }

                        break;
                    case FindOption.ContainName:
                        if (obj.name.Contains(pathList[i]))
                        {
                            if (i + 1 == pathList.Length)
                            {
                                objects.Add(obj);
                            }
                            else
                            {
                                newOptionList.Add(i + 1);
                            }
                        }

                        break;
                    case FindOption.Component
                        : //For finding object that contains object we don't need to search in hierarchy
                        break;
                }

            }

            optionList = newOptionList;

            switch (option)
            {
                case FindOption.Name:
                    if (obj.name.Equals(pathList[0])||(pathList.Length==1&&pathList[0].Equals("")))
                    {
                        if (pathList.Length == 1)
                        {
                            objects.Add(obj);
                        }
                        else
                        {
                            optionList.Add(1);

                        }
                    }

                    break;
                case FindOption.ContainName:
                    if (obj.name.Contains(pathList[0]))
                    {
                        if (pathList.Length == 1)
                        {
                            objects.Add(obj);
                        }
                        else
                        {
                            optionList.Add(1);

                        }
                    }

                    break;
                case FindOption.Component:
                    if (obj.GetComponent(componentType) != null)
                    {
                        objects.Add(obj);
                    }

                    break;
            }


            foreach (Transform child in obj.transform)
            {
                List<GameObject> objecsFounds = CheckChildrenForMultipleGameObjects(child.gameObject, pathList,
                    optionList, enabled, option, componentType);
                objects.AddRange(objecsFounds);
            }
        }

        return objects;
    }

    private void FindObjectByName(string stringSent, AltClientSocketHandler handler)
    {
        var pieces = stringSent.Split(';');
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                GameObject foundGameObject = FindObjectInScene(objectName, enabled,FindOption.Name);
                if (foundGameObject != null)
                {
                    if (cameraName.Equals(""))
                        response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject));
                    else
                    {
                        Camera camera = Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                        response = camera == null ? errorNotFoundMessage : JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject, camera));
                    }
                }
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }

        });
    }

    private void FindObjectWhereNameContains(string methodParameters, AltClientSocketHandler handler)
    {
        var pieces = methodParameters.Split(';');
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                GameObject foundGameObject = FindObjectInScene(objectName, enabled, FindOption.ContainName);
                if (foundGameObject != null)
                {
                    if (cameraName.Equals(""))
                        response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject));
                    else
                    {
                        Camera camera = Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                        response = camera == null ? errorNotFoundMessage : JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject, camera));
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }

            handler.SendResponse(response);
        });
    }

    private void FindObjectByComponent(string methodParameters, AltClientSocketHandler handler)
    {
        var pieces = methodParameters.Split(';');
        string assemblyName = pieces[0];
        string componentTypeName = pieces[1];
        string cameraName = pieces[2];
        bool enabled = Convert.ToBoolean(pieces[3]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                Type componentType = GetType(componentTypeName, assemblyName);
                if (componentType != null)
                {
                    GameObject foundGameObject = FindObjectInScene("", enabled, FindOption.Component, componentType);
                    if (foundGameObject != null)
                    {
                        if (cameraName.Equals(""))
                            response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject));
                        else
                        {
                            Camera camera = Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                            response = camera == null ? errorNotFoundMessage : JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject, camera));
                        }
                    }
                }
                else
                {
                    response = errorComponentNotFoundMessage;
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);

            }

        });
    }

    private void FindObjectsByName(string methodParameters, AltClientSocketHandler handler)
    {
        var pieces = methodParameters.Split(';');
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                List<AltUnityObject> foundObjects = new List<AltUnityObject>();

                foreach (GameObject testableObject in FindObjectsInScene(objectName, enabled,FindOption.Name))
                {

                    if (cameraName == null)
                        foundObjects.Add(GameObjectToAltUnityObject(testableObject));
                    else
                    {
                        Camera camera = Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                        foundObjects.Add(GameObjectToAltUnityObject(testableObject, camera));
                    }

                }

                response = JsonConvert.SerializeObject(foundObjects);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }

            finally
            {
                handler.SendResponse(response);

            }
        });
    }

    private void FindObjectsByComponent(string methodParameters, AltClientSocketHandler handler)
    {
        var pieces = methodParameters.Split(';');
        string assemblyName = pieces[0];
        string componentTypeName = pieces[1];
        string cameraName = pieces[2];
        bool enabled = Convert.ToBoolean(pieces[3]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                List<AltUnityObject> foundObjects = new List<AltUnityObject>();
                Type componentType = GetType(componentTypeName, assemblyName);
                if (componentType != null)
                {
                    foreach (GameObject testableObject in FindObjectsInScene("",enabled,FindOption.Component,componentType))
                    {
                        
                            if (cameraName == null)
                                foundObjects.Add(GameObjectToAltUnityObject(testableObject));
                            else
                            {
                                Camera camera = Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                                foundObjects.Add(GameObjectToAltUnityObject(testableObject, camera));
                            }
                        
                    }

                    response = JsonConvert.SerializeObject(foundObjects);
                }
                else
                {
                    response = errorComponentNotFoundMessage;
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }

            finally
            {
                handler.SendResponse(response);

            }
        });
    }

    private void FindObjectsWhereNameContains(string methodParameters, AltClientSocketHandler handler)
    {
        var pieces = methodParameters.Split(';');
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                List<AltUnityObject> foundObjects = new List<AltUnityObject>();
                foreach (GameObject testableObject in FindObjectsInScene("", enabled, FindOption.ContainName))
                {
                    if (enabled && !testableObject.activeInHierarchy) continue;
                    if (testableObject.name.Contains(objectName))
                    {
                        if (cameraName == null)
                            foundObjects.Add(GameObjectToAltUnityObject(testableObject));
                        else
                        {
                            Camera camera = Camera.allCameras.ToList().Find(c => c.name.Equals(cameraName));
                            foundObjects.Add(GameObjectToAltUnityObject(testableObject, camera));
                        }
                    }
                }

                response = JsonConvert.SerializeObject(foundObjects);

            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
            AltUnityObject scene = new AltUnityObject(name: SceneManager.GetActiveScene().name,
                                                       type: "UnityScene");
            handler.SendResponse(JsonUtility.ToJson(scene));
        });
    }


    private void ClickEvent(AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                GameObject foundGameObject = GetGameObject(altUnityObject);
                var pointerEventData = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(foundGameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
                response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundGameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                Touch touch = new Touch { position = new Vector2(float.Parse(x), float.Parse(y)) };
                var pointerEventData = mockUp.GetPointerEventData(touch);
                GameObject gameObject = pointerEventData.pointerPress.gameObject;
                Debug.Log("GameOBject: " + gameObject);

                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
                gameObject.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.initializePotentialDrag);
                gameObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.submitHandler);
                gameObject.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                gameObject.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);

                response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject, pointerEventData.enterEventCamera));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                AltUnityObjectProperty altProperty = JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
                AltUnityObject altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                GameObject testableObject = GetGameObject(altUnityObject);
                MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
                response = GetValueForMember(memberInfo, testableObject, altProperty);
            }
            catch (JsonException e)
            {
                Debug.Log(e);
                if (altObjectString.Contains("error"))
                {
                    response = errorObjectWasNotFound;
                }
                else
                {
                    response = errorCouldNotParseJsonString;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
                response = errorComponentNotFoundMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
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
                    JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
                AltUnityObject altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                GameObject testableObject = GetGameObject(altUnityObject);
                MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
                response = SetValueForMember(memberInfo, valueString, testableObject, altProperty);
            }
            catch (JsonException e)
            {
                Debug.Log(e);
                if (altObjectString.Contains("error"))
                {
                    response = errorObjectWasNotFound;
                }
                else
                {
                    response = errorCouldNotParseJsonString;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
                response = errorComponentNotFoundMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
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
                MethodInfo methodInfoToBeInvoked;
                AltUnityObjectAction altAction = JsonConvert.DeserializeObject<AltUnityObjectAction>(actionString);
                var componentType = GetType(altAction.Component, altAction.Assembly);

                MethodInfo[] methodInfos = GetMethodInfoWithSpecificName(componentType, altAction.Method);
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
                    AltUnityObject altObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                    GameObject gameObject = GetGameObject(altObject);
                    if (componentType == typeof(GameObject))
                    {
                        response = InvokeMethod(methodInfoToBeInvoked, altAction, gameObject, response);
                    }
                    else
                    if (gameObject.GetComponent(componentType) != null)
                    {
                        Component component = gameObject.GetComponent(componentType);
                        response = InvokeMethod(methodInfoToBeInvoked, altAction, component, response);
                    }
                }
            }
            catch (ArgumentException)
            {
                response = errorFailedToParseArguments;
            }
            catch (TargetParameterCountException)
            {
                response = errorIncorrectNumberOfParameters;
            }
            catch (JsonException e)
            {
                Debug.Log(e);
                response = altObjectString.Contains("error") ? errorObjectWasNotFound : errorCouldNotParseJsonString;
            }
            catch (NullReferenceException)
            {
                response = errorComponentNotFoundMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            handler.SendResponse(response);
        });
    }

    private MethodInfo[] GetMethodInfoWithSpecificName(Type componentType, string altActionMethod)
    {
        MethodInfo[] methodInfos = componentType.GetMethods();
        return methodInfos.Where(method => method.Name.Equals(altActionMethod)).ToArray();
    }

    private MethodInfo GetMethodToBeInvoked(MethodInfo[] methodInfos, AltUnityObjectAction altUnityObjectAction)
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
                    Type type = Type.GetType(typeOfParametes[counter]);
                    if (methodInfo.GetParameters()[counter].ParameterType != type)
                        throw new Exception("Missmatch in parameter type");
                }
                //If every parameter can be deserialize then this is our method(except if there int but method can take also int)
                return methodInfo;

            }
            catch (Exception e)
            {

            }

        }

        var errorMessage = "No method found with this signature: " + altUnityObjectAction.Method + "(";
        errorMessage = typeOfParametes.Aggregate(errorMessage, (current, typeOfParamete) => current + (typeOfParamete + ","));

        errorMessage = errorMessage.Remove(errorMessage.Length - 1);
        errorMessage += ")";
        throw new Exception(errorMessage);
    }

    private static string InvokeMethod(MethodInfo methodInfo, AltUnityObjectAction altAction, object component, string response)
    {
        if (methodInfo == null) return response;
        if (altAction.Parameters == "")
        {
            response = JsonConvert.SerializeObject(methodInfo.Invoke(component, null));
        }
        else
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            string[] parameterStrings = altAction.Parameters.Split('?');
            if (parameterInfos.Length != parameterStrings.Length)
                throw new TargetParameterCountException();
            object[] parameters = new object[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                if (parameterInfos[i].ParameterType == typeof(System.String))
                    parameters[i] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(parameterStrings[i]),
                        parameterInfos[i].ParameterType);
                else
                {
                    parameters[i] = JsonConvert.DeserializeObject(parameterStrings[i], parameterInfos[i].ParameterType);
                }
            }

            response = JsonConvert.SerializeObject(methodInfo.Invoke(component, parameters));
        }
        return response;
    }

    private void CloseConnection(AltClientSocketHandler handler)
    {
        _socketServer.StartListeningForConnections();

    }

    private void UnknownString(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            handler.SendResponse(errorCouldNotPerformOperationMessage);
        });
    }

    void Update()
    {
        _responseQueue.Cycle();
    }

    public static Type GetType(string typeName, string assemblyName)
    {
        var type = Type.GetType(typeName);

        if (type != null)
            return type;
        if (assemblyName == null || assemblyName.Equals(""))
        {
            if (typeName.Contains("."))
            {
                assemblyName = typeName.Substring(0, typeName.LastIndexOf('.'));
                Debug.Log("assembly name " + assemblyName);
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    if (assembly.GetType(typeName) == null)
                        return null;
                    return assembly.GetType(typeName);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    return null;
                }
            }

            return null;
        }
        else
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly.GetType(typeName) == null)
                    return null;
                return assembly.GetType(typeName);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return null;
            }

        }
    }

    private static GameObject GetGameObject(AltUnityObject altUnityObject)
    {
        foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (gameObject.GetInstanceID() == altUnityObject.id)
                return gameObject;
        }
        return null;
    }
    private static GameObject GetGameObject(int objectId)
    {
        foreach (GameObject gameObject in FindObjectsOfType<GameObject>())
        {
            if (gameObject.GetInstanceID() == objectId)
                return gameObject;
        }
        return null;
    }

    private MemberInfo GetMemberForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectProperty altUnityObjectProperty)
    {
        MemberInfo memberInfo = null;
        Type componentType = null;
        componentType = GetType(altUnityObjectProperty.Component, altUnityObjectProperty.Assembly);
        PropertyInfo propertyInfo = componentType.GetProperty(altUnityObjectProperty.Property);
        FieldInfo fieldInfo = componentType.GetField(altUnityObjectProperty.Property);
        if (GetGameObject(altUnityObject).GetComponent(componentType) != null)
        {
            if (propertyInfo != null)
                return propertyInfo;
            if (fieldInfo != null)
                return fieldInfo;
        }
        return memberInfo;
    }


    private MethodInfo GetMethodForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectAction altUnityObjectAction)
    {
        Type componentType = null;
        componentType = GetType(altUnityObjectAction.Component, altUnityObjectAction.Assembly);
        MethodInfo methodInfo = componentType.GetMethod(altUnityObjectAction.Method);
        return methodInfo;
    }

    private string GetValueForMember(MemberInfo memberInfo, GameObject testableObject, AltUnityObjectProperty altProperty)
    {
        string response = errorPropertyNotFoundMessage;
        if (memberInfo != null)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                object value = propertyInfo.GetValue(testableObject.GetComponent(GetType(altProperty.Component, altProperty.Assembly)), null);
                response = SerializeMemberValue(value, propertyInfo.PropertyType);
            }
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                object value = fieldInfo.GetValue(testableObject.GetComponent(GetType(altProperty.Component, altProperty.Assembly)));
                response = SerializeMemberValue(value, fieldInfo.FieldType);
            }
        }
        return response;
    }

    private string SetValueForMember(MemberInfo memberInfo, string valueString, GameObject testableObject, AltUnityObjectProperty altProperty)
    {
        string response = errorPropertyNotFoundMessage;
        if (memberInfo != null)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
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
                catch (Exception e)
                {
                    Debug.Log(e);
                    response = errorPropertyNotSet;
                }
            }
            if (memberInfo.MemberType == MemberTypes.Field)
            {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
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
                catch (Exception e)
                {
                    Debug.Log(e);
                    response = errorPropertyNotSet;
                }
            }
        }
        return response;
    }

    private string SerializeMemberValue(object value, Type type)
    {
        string response;
        if (type == typeof(string))
            return value.ToString();
        try
        {
            response = JsonConvert.SerializeObject(value, type, _jsonSettings);
        }
        catch (JsonException)
        {
            response = value.ToString();
        }
        return response;
    }

    private object DeserializeMemberValue(string valueString, Type type)
    {
        object value;
        if (type == typeof(string))
            valueString = JsonConvert.SerializeObject(valueString);
        try
        {
            value = JsonConvert.DeserializeObject(valueString, type);
        }
        catch (JsonException)
        {
            value = null;
        }
        return value;
    }
#if ALTUNITYTESTER

    private void SetMovingTouch(Vector2 start, Vector2 destination, string duration, AltClientSocketHandler handler)
    {

        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                Touch touch = new Touch();
                touch.phase = TouchPhase.Began;
                touch.position = start;
                List<Touch> touches = Input.touches.ToList();
                touches.Sort((touch1, touch2) => (touch1.fingerId.CompareTo(touch2.fingerId)));
                int fingerId = 0;
                foreach (Touch iter in touches)
                {
                    if (iter.fingerId != fingerId)
                        break;
                    fingerId++;
                }

                touch.fingerId = fingerId;
                Input.SetMovingTouch(touch, destination, float.Parse(duration));
                response = "Ok";
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });

    }
#endif
    private void DragObject(Vector2 position, AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                MockUpPointerInputModule mockUp = new MockUpPointerInputModule();
                var pointerEventData = mockUp.GetPointerEventData(new Touch() { position = position });
                GameObject gameObject = GetGameObject(altUnityObject);
                Camera viewingCamera = FoundCameraById(altUnityObject.idCamera);
                Vector3 gameObjectPosition = viewingCamera.WorldToScreenPoint(gameObject.transform.position);
                pointerEventData.delta = pointerEventData.position - new Vector2(gameObjectPosition.x, gameObjectPosition.y);
                Debug.Log("GameOBject: " + gameObject);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.dragHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
    private void DropObject(Vector2 position, AltUnityObject altUnityObject, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                var pointerEventData = new PointerEventData(EventSystem.current);
                GameObject gameObject = GetGameObject(altUnityObject);
                Debug.Log("GameOBject: " + gameObject);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.dropHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                var pointerEventData = new PointerEventData(EventSystem.current);
                GameObject gameObject = GetGameObject(altUnityObject);
                Debug.Log("GameOBject: " + gameObject);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                var pointerEventData = new PointerEventData(EventSystem.current);
                GameObject gameObject = GetGameObject(altUnityObject);
                Debug.Log("GameOBject: " + gameObject);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                if (camera != null)
                {
                    response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject, camera));
                }
                else
                {

                    response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject));
                }
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                var pointerEventData = new PointerEventData(EventSystem.current);
                GameObject gameObject = GetGameObject(altUnityObject);
                Debug.Log("GameOBject: " + gameObject);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                var pointerEventData = new PointerEventData(EventSystem.current);
                GameObject gameObject = GetGameObject(altUnityObject);
                Debug.Log("GameOBject: " + gameObject);
                ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                var camera = FoundCameraById(altUnityObject.idCamera);
                response = JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
#if ALTUNITYTESTER
    private void Tilt(Vector3 acceleration, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                Input.acceleration = acceleration;
                response = "OK";
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
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

                SceneManager.LoadScene(scene);
                response = "Ok";

            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
    //        Debug.Log("Scenesnumber:"+SceneManager.sceneCountInBuildSettings);
    //        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
    //        {
    //            Debug.Log(SceneManager.GetSceneByBuildIndex(i));
    //            if (SceneManager.GetSceneByBuildIndex(i).name.Equals(scene))
    //                return true;
    //        }
    //
    //        return false;
    //    }

    private Camera FoundCameraById(int id)
    {
        foreach (var camera in Camera.allCameras)
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
                PlayerPrefs.DeleteAll();
                response = "Ok";
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                PlayerPrefs.DeleteKey(keyName);
                response = "Ok";
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                        Debug.Log("Set Option String ");
                        PlayerPrefs.SetString(keyName, valueName);
                        break;
                    case PLayerPrefKeyType.Float:
                        Debug.Log("Set Option Float ");
                        PlayerPrefs.SetFloat(keyName, float.Parse(valueName));
                        break;
                    case PLayerPrefKeyType.Int:
                        Debug.Log("Set Option Int ");
                        PlayerPrefs.SetInt(keyName, int.Parse(valueName));
                        break;
                }

                response = "Ok";
            }
            catch (FormatException exception)
            {
                Debug.Log(exception);
                response = errorFormatException;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
                if (PlayerPrefs.HasKey(keyName))
                {
                    switch (option)
                    {
                        case PLayerPrefKeyType.String:
                            Debug.Log("Option String " + PlayerPrefs.GetString(keyName));
                            response = PlayerPrefs.GetString(keyName);
                            break;
                        case PLayerPrefKeyType.Float:
                            Debug.Log("Option Float " + PlayerPrefs.GetFloat(keyName));
                            response = PlayerPrefs.GetFloat(keyName) + "";
                            break;
                        case PLayerPrefKeyType.Int:
                            Debug.Log("Option Int " + PlayerPrefs.GetInt(keyName));
                            response = PlayerPrefs.GetInt(keyName) + "";
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            }
            finally
            {
                handler.SendResponse(response);
            }
        });
    }
#if ALTUNITYTESTER
    private void SwipeFinished(AltClientSocketHandler handler)
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
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
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
                var pointerEventData = new PointerEventData(EventSystem.current);
                GameObject gameObject = GetGameObject(altUnityObject);
                Debug.Log("GameOBject: " + gameObject);

                ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, ExecuteEvents.pointerEnterHandler);
                gameObject.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
                gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, ExecuteEvents.initializePotentialDrag);
                gameObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
                gameObject.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
                ExecuteEvents.ExecuteHierarchy(gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                gameObject.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);

                var camera = FoundCameraById(altUnityObject.idCamera);
                response = JsonConvert.SerializeObject(camera != null ? GameObjectToAltUnityObject(gameObject, camera) : GameObjectToAltUnityObject(gameObject));
            }
            catch (NullReferenceException exception)
            {
                Debug.Log(exception);
                response = errorNullRefferenceMessage;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
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
            GameObject altObject = GetGameObject(Convert.ToInt32(ObjectId));
            List<AltUnityComponent> listComponents=new List<AltUnityComponent>();
            foreach (var component in altObject.GetComponents<Component>())
            {
                var a = component.GetType();
                var componentName = a.FullName;
                var assemblyName = a.Assembly.GetName().Name;
                listComponents.Add(new AltUnityComponent(componentName,assemblyName));
            }

            var response = JsonConvert.SerializeObject(listComponents);
            handler.SendResponse(response);
        });
    }
    private void GetAllProperties(string id,AltUnityComponent component, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            GameObject altObject;
            altObject = id.Equals("null") ? null : GetGameObject(Convert.ToInt32(id));
            Type type = GetType(component.componentName, component.assemblyName);
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            List<AltUnityProperty> listProperties = new List<AltUnityProperty>();

            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    var value = propertyInfo.GetValue(altObject.GetComponent(type), null);
                    listProperties.Add(new AltUnityProperty(propertyInfo.Name,
                        value == null ? "null" : value.ToString()));
                }catch (Exception e)
                {
                   Debug.Log(e.StackTrace);
                }
            }
            handler.SendResponse(JsonConvert.SerializeObject(listProperties));
        });
    }

    private void GetAllMethods(AltUnityComponent component, AltClientSocketHandler handler)
    {

        _responseQueue.ScheduleResponse(delegate
        {
                Type type = GetType(component.componentName, component.assemblyName);
                var methodInfos = type.GetMembers(BindingFlags.Public |BindingFlags.NonPublic| BindingFlags.Instance);

                List<string> listProperties = new List<string>();

                foreach (var methodInfo in methodInfos)
                {
                    listProperties.Add(methodInfo.ToString());
                }
                handler.SendResponse(JsonConvert.SerializeObject(listProperties));
        });
    }
}
