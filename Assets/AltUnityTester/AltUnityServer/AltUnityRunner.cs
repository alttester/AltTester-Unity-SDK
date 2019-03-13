using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Reflection;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.UI;
using Unity.IO.Compression;

public class AltUnityRunner : MonoBehaviour, AltIClientSocketHandlerDelegate
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

    private bool destroyHightlight = false; 

    enum FindOption
    {
        Name, ContainName, Component
    }
    public int SocketPortNumber = 13000;
    public bool DebugBuildNeeded = true;

    public Shader outlineShader;
    public GameObject panelHightlightPrefab;

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
        AltUnityEvents.Instance.GetAllFields.AddListener(GetAllFields);
        AltUnityEvents.Instance.GetAllScenes.AddListener(GetAllScenes);
        AltUnityEvents.Instance.GetAllCameras.AddListener(GetAllCameras);

        AltUnityEvents.Instance.GetScreenshot.AddListener(GetScreenshot);
        AltUnityEvents.Instance.HighlightObjectScreenshot.AddListener(HighLightSelectedObject);
        AltUnityEvents.Instance.HighlightObjectFromCoordinates.AddListener(HightObjectFromCoordinates);
        AltUnityEvents.Instance.ScreenshotReady.AddListener(ScreenshotReady);

        AltUnityEvents.Instance.SetTimeScale.AddListener(SetTimeScale);
        AltUnityEvents.Instance.GetTimeScale.AddListener(GetTimeScale);

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

    public void StartSocketServer()
    {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;
        
        Encoding encoding = Encoding.UTF8;

        _socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, SocketPortNumber, maxClients, requestEndingString, encoding);

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


    private Vector3 getObjectScreePosition(GameObject gameObject, Camera camera)
    {
        Canvas canvasParent = gameObject.GetComponentInParent<Canvas>();
        if (canvasParent != null)
        {
            if (canvasParent.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                if (gameObject.GetComponent<RectTransform>() == null)
                {
                    return canvasParent.worldCamera.WorldToScreenPoint(gameObject.transform.position);
                }
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


    private AltUnityObject GameObjectToAltUnityObject(GameObject altGameObject, Camera camera = null)
    {
        int cameraId = -1;
        //if no camera is given it will iterate through all cameras until  found one that sees the object if no camera sees the object it will return the position from the last camera
        //if there is no camera in the scene it will return as scren position x:-1 y=-1, z=-1 and cameraId=-1
        if (camera == null)
        {
            _position = new Vector3(-1, -1, -1);
            foreach (var camera1 in Camera.allCameras)
            {
                _position = getObjectScreePosition(altGameObject, camera1);
                cameraId = camera1.GetInstanceID();
                if (_position.x > 0 && _position.y > 0 && _position.x < Screen.width && _position.y < Screen.height && _position.z >= 0)//Check if camera sees the object
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
                                                      x: Convert.ToInt32(Mathf.Round(_position.x)),
                                                      y: Convert.ToInt32(Mathf.Round(_position.y)),
                                                      z: Convert.ToInt32(Mathf.Round(_position.z)),//if z is negative object is behind the camera
                                                      mobileY: Convert.ToInt32(Mathf.Round(Screen.height - _position.y)),
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
        string[] pieces = message.Split(separator, StringSplitOptions.None);
        AltUnityComponent altComponent;
        AltUnityObject altUnityObject;
        string methodParameters;
        Vector2 size;
        PLayerPrefKeyType option;
        switch (pieces[0])
        {
            case "findAllObjects":
                Debug.Log("all objects requested");
                methodParameters = pieces[1] + requestSeparatorString + pieces[2];
                AltUnityEvents.Instance.GetAllObjects.Invoke(methodParameters, handler);
                break;
            case "findObjectByName":
                Debug.Log("find object by name " + pieces[1]);
                Debug.Log(pieces.Length);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectByName.Invoke(methodParameters, handler);
                break;
            case "findObjectWhereNameContains":
                Debug.Log("find object where name contains:" + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
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
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectsByName.Invoke(methodParameters, handler);
                break;
            case "findObjectsWhereNameContains":
                Debug.Log("find objects where name contains:" + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3];
                AltUnityEvents.Instance.FindObjectsWhereNameContains.Invoke(methodParameters, handler);
                break;
            case "getCurrentScene":
                Debug.Log("get current scene");
                AltUnityEvents.Instance.GetCurrentScene.Invoke(handler);
                break;
            case "findObjectByComponent":
                Debug.Log("find object by component " + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3] + requestSeparatorString + pieces[4];
                AltUnityEvents.Instance.FindObjectByComponent.Invoke(methodParameters, handler);
                break;
            case "findObjectsByComponent":
                Debug.Log("find objects by component " + pieces[1]);
                methodParameters = pieces[1] + requestSeparatorString + pieces[2] + requestSeparatorString + pieces[3] + requestSeparatorString + pieces[4];
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
            case "setTimeScale":
                Debug.Log("SetTimeScale");
                float timeScale = JsonConvert.DeserializeObject<float>(pieces[1]);
                AltUnityEvents.Instance.SetTimeScale.Invoke(timeScale, handler);
                break;
            case "getTimeScale":
                Debug.Log("GetTimeScale");
                AltUnityEvents.Instance.GetTimeScale.Invoke(handler);
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
                AltUnityEvents.Instance.GetAllComponents.Invoke(pieces[1], handler);
                break;
            case "getAllFields":
                Debug.Log("getAllFields");
                altComponent = JsonConvert.DeserializeObject<AltUnityComponent>(pieces[2]);
                AltUnityEvents.Instance.GetAllFields.Invoke(pieces[1], altComponent, handler);
                break;
            case "getAllMethods":
                Debug.Log("getAllMethods");
                altComponent = JsonConvert.DeserializeObject<AltUnityComponent>(pieces[1]);
                AltUnityEvents.Instance.GetAllMethods.Invoke(altComponent, handler);
                break;
            case "getAllScenes":
                Debug.Log("getAllScenes");
                AltUnityEvents.Instance.GetAllScenes.Invoke(handler);
                break;
            case "getAllCameras":
                Debug.Log("getAllCameras");
                AltUnityEvents.Instance.GetAllCameras.Invoke(handler);
                break;
            case "getScreenshot":
                Debug.Log("getScreenshot" + pieces[1]);
                //                var size = new Vector2(Convert.ToInt32(pieces[1]),Convert.ToInt32(pieces[2]));
                size = JsonConvert.DeserializeObject<Vector2>(pieces[1]);
                AltUnityEvents.Instance.GetScreenshot.Invoke(size, handler);
                break;
            case "hightlightObjectScreenshot":
                Debug.Log("HightlightObject");
                var id = Convert.ToInt32(pieces[1]);
                size = JsonConvert.DeserializeObject<Vector2>(pieces[3]);
                AltUnityEvents.Instance.HighlightObjectScreenshot.Invoke(id, pieces[2], size, handler);
                break;
            case "hightlightObjectFromCoordinatesScreenshot":
                Debug.Log("HightlightObject");
                var coordinates = JsonConvert.DeserializeObject<Vector2>(pieces[1]);
                size = JsonConvert.DeserializeObject<Vector2>(pieces[3]);
                AltUnityEvents.Instance.HighlightObjectFromCoordinates.Invoke(coordinates, pieces[2], size, handler);
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
            DontDestroyOnLoad(temp);
            Scene dontDestroyOnLoad = temp.scene;
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
    private GameObject FindObjectInScene(string objectName, bool enabled)
    {
        string[] pathList = objectName.Split('/');
        GameObject foundGameObject = null;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            foreach (GameObject obj in SceneManager.GetSceneAt(i).GetRootGameObjects())
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
    private GameObject CheckChildren(GameObject obj, string[] pathList, bool enabled)
    {
        GameObject objectReturned = null;
        foreach (Transform childrenTransform in obj.transform)
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
    private GameObject CheckPath(GameObject obj, string[] pathList, int pathListStep, bool enabled)
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
                var id = Convert.ToInt32(pathList[pathListStep].Substring(3, pathList[pathListStep].Length - 4));
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
    private GameObject CheckNextElementInPath(GameObject obj, string[] pathList, int pathListStep, bool enabled)
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
            foreach (Transform childrenObject in obj.transform)
            {
                var objectReturned = CheckPath(childrenObject.gameObject, pathList, nextStep, enabled);
                if (objectReturned != null)
                    return objectReturned;
            }
            return null;
        }
    }
    private List<GameObject> FindObjectsInScene(string objectName, bool enabled)
    {
        List<GameObject> objectsFound = new List<GameObject>();
        string[] pathList = objectName.Split('/');
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            foreach (GameObject obj in SceneManager.GetSceneAt(i).GetRootGameObjects())
            {
                List<GameObject> listGameObjects = CheckPathForMultipleElements(obj.gameObject, pathList, 0, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
                listGameObjects = CheckChildrenForMultipleElements(obj.gameObject, pathList, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
            }
        }
        foreach (var destroyOnLoadObject in GetDontDestroyOnLoadObjects())
        {
            List<GameObject> listGameObjects = CheckPathForMultipleElements(destroyOnLoadObject.gameObject, pathList, 0, enabled);
            if (listGameObjects != null)
                objectsFound.AddRange(listGameObjects);
            listGameObjects = CheckChildrenForMultipleElements(destroyOnLoadObject.gameObject, pathList, enabled);
            objectsFound.AddRange(listGameObjects);
        }
        return objectsFound;
    }
    private List<GameObject> CheckChildrenForMultipleElements(GameObject obj, string[] pathList, bool enabled)
    {
        List<GameObject> objectsFound = new List<GameObject>();
        foreach (Transform childrenTransform in obj.transform)
        {
            List<GameObject> listGameObjects = CheckPathForMultipleElements(childrenTransform.gameObject, pathList, 0, enabled);
            if (listGameObjects != null)
                objectsFound.AddRange(listGameObjects);
            listGameObjects = CheckChildrenForMultipleElements(childrenTransform.gameObject, pathList, enabled);
            if (listGameObjects != null)
                objectsFound.AddRange(listGameObjects);

        }
        return objectsFound;
    }
    private List<GameObject> CheckPathForMultipleElements(GameObject obj, string[] pathList, int pathListStep, bool enabled)
    {
        List<GameObject> objectsFound = new List<GameObject>();
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
                var id = Convert.ToInt32(pathList[pathListStep].Substring(3, pathList[pathListStep].Length - 4));
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
    private List<GameObject> CheckNextElementInPathForMultipleElements(GameObject obj, string[] pathList, int pathListStep, bool enabled)
    {
        List<GameObject> objectsFound = new List<GameObject>();
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
                List<GameObject> listGameObjects = CheckPathForMultipleElements(obj, pathList, nextStep, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
                return objectsFound;
            }
            foreach (Transform childrenObject in obj.transform)
            {
                List<GameObject> listGameObjects = CheckPathForMultipleElements(childrenObject.gameObject, pathList, nextStep, enabled);
                if (listGameObjects != null)
                    objectsFound.AddRange(listGameObjects);
            }
            return objectsFound;
        }
    }




    private void FindObjectByName(string stringSent, AltClientSocketHandler handler) {
        var pieces = stringSent.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                GameObject foundGameObject = FindObjectInScene(objectName, enabled);
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

    private void FindObjectWhereNameContains(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                foreach (GameObject testableObject in FindObjectsOfType<GameObject>())
                {
                    if (testableObject.name.Contains(objectName))
                    {
                        response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(testableObject));
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                response = errorUnknownError + requestSeparatorString + exception;
            }

            handler.SendResponse(response);
        });

    }

    private void FindObjectByComponent(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);
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
                    foreach (GameObject testableObject in FindObjectsOfType<GameObject>())
                    {
                        if (testableObject.GetComponent(componentType) != null)
                        {
                            var foundObject = testableObject;
                            response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(foundObject));
                            break;
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

    private void FindObjectsByName(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                List<AltUnityObject> foundObjects = new List<AltUnityObject>();
                foreach (GameObject testableObject in FindObjectsInScene(objectName, enabled))
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

    private void FindObjectsByComponent(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);
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
                    foreach (GameObject testableObject in FindObjectsOfType<GameObject>())
                    {
                        if (name == "" || testableObject.GetComponent(componentType) != null)
                        {
                            foundObjects.Add(GameObjectToAltUnityObject(testableObject));
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

    private void FindObjectsWhereNameContains(string methodParameters, AltClientSocketHandler handler) {
        var pieces = methodParameters.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);
        string objectName = pieces[0];
        string cameraName = pieces[1];
        bool enabled = Convert.ToBoolean(pieces[2]);
        _responseQueue.ScheduleResponse(delegate
        {
            string response = errorNotFoundMessage;
            try
            {
                List<AltUnityObject> foundObjects = new List<AltUnityObject>();
                foreach (GameObject testableObject in FindObjectsOfType<GameObject>())
                {
                    if (testableObject.name.Contains(objectName))
                    {
                        foundObjects.Add(GameObjectToAltUnityObject(testableObject));
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
                Touch touch = new Touch { position = new Vector2(float.Parse(x), float.Parse(y)), phase = TouchPhase.Began};
                var pointerEventData = mockUp.ExecuteTouchEvent(touch);
                if (pointerEventData.pointerPress == null &&
                    pointerEventData.pointerEnter == null &&
                    pointerEventData.pointerDrag == null)
                {
                    response = errorNotFoundMessage;
                }
                else
                {
                    GameObject gameObject = pointerEventData.pointerPress.gameObject;

                    Debug.Log("GameOBject: " + gameObject);

                    gameObject.SendMessage("OnMouseEnter", SendMessageOptions.DontRequireReceiver);
                    gameObject.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
                    gameObject.SendMessage("OnMouseOver", SendMessageOptions.DontRequireReceiver);
                    ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerUpHandler);
                    gameObject.SendMessage("OnMouseUp", SendMessageOptions.DontRequireReceiver);
                    gameObject.SendMessage("OnMouseUpAsButton", SendMessageOptions.DontRequireReceiver);
                    ExecuteEvents.Execute(gameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
                    gameObject.SendMessage("OnMouseExit", SendMessageOptions.DontRequireReceiver);
                    touch.phase = TouchPhase.Ended;
                    mockUp.ExecuteTouchEvent(touch, pointerEventData);

                    response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject, pointerEventData.enterEventCamera));
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
                response = errorUnknownError + requestSeparatorString + exception;
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
        Debug.Log("Close connection event handler!");
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
                response = errorUnknownError + requestSeparatorString + exception;
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
                var pointerEventData = mockUp.ExecuteTouchEvent(new Touch() { position = position });
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
            List<AltUnityComponent> listComponents = new List<AltUnityComponent>();
            foreach (var component in altObject.GetComponents<Component>())
            {
                var a = component.GetType();
                var componentName = a.FullName;
                var assemblyName = a.Assembly.GetName().Name;
                listComponents.Add(new AltUnityComponent(componentName, assemblyName));
            }

            var response = JsonConvert.SerializeObject(listComponents);
            handler.SendResponse(response);
        });
    }
    private void GetAllFields(string id, AltUnityComponent component, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            GameObject altObject;
            altObject = id.Equals("null") ? null : GetGameObject(Convert.ToInt32(id));
            Type type = GetType(component.componentName, component.assemblyName);
            var altObjectComponent = altObject.GetComponent(type);
            var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            List<AltUnityField> listFields = new List<AltUnityField>();

            foreach (var fieldInfo in fieldInfos)
            {
                try
                {
                    var value = fieldInfo.GetValue(altObjectComponent);
                    listFields.Add(new AltUnityField(fieldInfo.Name,
                        value == null ? "null" : value.ToString()));
                }
                catch (Exception e)
                {
                    Debug.Log(e.StackTrace);
                }
            }
            handler.SendResponse(JsonConvert.SerializeObject(listFields));
        });
    }

    private void GetAllMethods(AltUnityComponent component, AltClientSocketHandler handler)
    {

        _responseQueue.ScheduleResponse(delegate
        {
            Type type = GetType(component.componentName, component.assemblyName);
            var methodInfos = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            List<string> listMethods = new List<string>();

            foreach (var methodInfo in methodInfos)
            {
                listMethods.Add(methodInfo.ToString());
            }
            handler.SendResponse(JsonConvert.SerializeObject(listMethods));
        });
    }
    private void GetAllScenes(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            List<String> SceneNames = new List<string>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var s = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                SceneNames.Add(s);
            }
            handler.SendResponse(JsonConvert.SerializeObject(SceneNames));
        });

    }
    private void GetAllCameras(AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var cameras = FindObjectsOfType<Camera>();
            List<string> cameraNames = new List<string>();
            foreach (Camera camera in cameras)
            {
                cameraNames.Add(camera.name);
            }
            handler.SendResponse(JsonConvert.SerializeObject(cameraNames));
        });
    }

    private void SetTimeScale(float timeScale, AltClientSocketHandler handler) {
        _responseQueue.ScheduleResponse(delegate {
            string response = errorCouldNotPerformOperationMessage;
            try {
                Time.timeScale = timeScale;
                response = "Ok";
            } catch (Exception exception) {
                Debug.Log(exception);
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
                response = JsonConvert.SerializeObject(Time.timeScale);
            } catch (Exception exception) {
                Debug.Log(exception);
                response = errorUnknownError + ";" + exception;
            } finally {
                handler.SendResponse(response);

            }
        });
    }

    private void HightObjectFromCoordinates(Vector2 screenCoordinates, string ColorAndWidth, Vector2 size, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);

            Color color = new Color(red, green, blue, alpha);
            float width = float.Parse(pieces[1]);

            Ray ray = Camera.main.ScreenPointToRay(screenCoordinates);
            RaycastHit[] hits;
            var raycasters = FindObjectsOfType<GraphicRaycaster>();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = screenCoordinates;
            foreach (var raycaster in raycasters)
            {
                List<RaycastResult> hitUI = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, hitUI);
                foreach (var hit in hitUI)
                {
                    StartCoroutine(HighLightSelectedObjectCorutine(hit.gameObject, color, width, size, handler));
                    return;
                }
            }
            hits = Physics.RaycastAll(ray);
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
    private void HighLightSelectedObject(int id, string ColorAndWidth, Vector2 size, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate
        {
            var pieces = ColorAndWidth.Split(new[] { "!-!" }, StringSplitOptions.None);
            var piecesColor = pieces[0].Split(new[] { "!!" }, StringSplitOptions.None);
            float red = float.Parse(piecesColor[0]);
            float green = float.Parse(piecesColor[1]);
            float blue = float.Parse(piecesColor[2]);
            float alpha = float.Parse(piecesColor[3]);

            Color color = new Color(red, green, blue, alpha);
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
    IEnumerator HighLightSelectedObjectCorutine(GameObject gameObject, Color color, float width, Vector2 size, AltClientSocketHandler handler)
    {
        destroyHightlight = false;
        Renderer renderer = gameObject.GetComponent<Renderer>();
        List<Shader> originalShaders = new List<Shader>();
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
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                var panelHighlight = Instantiate(panelHightlightPrefab, rectTransform);
                panelHighlight.GetComponent<Image>().color = color;
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
    private void GetScreenshot(Vector2 size, AltClientSocketHandler handler)
    {
        _responseQueue.ScheduleResponse(delegate {
            StartCoroutine(TakeScreenshot(size, handler));
        });
        
    }

    private void ScreenshotReady(Texture2D screenshot, Vector2 size, AltClientSocketHandler handler) {
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

            fullResponse[0] = JsonConvert.SerializeObject(new Vector2(screenshot.width, screenshot.height), new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            TextureScale.Bilinear(screenshot, width, height);
            screenshot.Apply(true);
            screenshot.Compress(false);
            screenshot.Apply(false);


            var screenshotSerialized = screenshot.GetRawTextureData();
            Debug.Log(screenshotSerialized.LongLength + " size after Unity Compression");
            Debug.Log(DateTime.Now + " Start Compression");
            var screenshotCompressed = CompressScreenshot(screenshotSerialized);
            Debug.Log(DateTime.Now + " Finished Compression");
            var length = screenshotCompressed.LongLength;
            fullResponse[1] = length.ToString();

            var format = screenshot.format;
            fullResponse[2] = format.ToString();

            var newSize = new Vector3(screenshot.width, screenshot.height);
            fullResponse[3] = JsonConvert.SerializeObject(newSize, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Debug.Log(DateTime.Now + " Serialize screenshot");
            fullResponse[4] = JsonConvert.SerializeObject(screenshotCompressed, new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });
        
            Debug.Log(DateTime.Now + " Finished Serialize Screenshot Start serialize response");
            handler.SendResponse(JsonConvert.SerializeObject(fullResponse));
            Debug.Log(DateTime.Now + " Finished send Response");
            Destroy(screenshot);
            destroyHightlight = true;
        });
    }

    private IEnumerator TakeScreenshot(Vector2 size, AltClientSocketHandler handler) {
        yield return new WaitForEndOfFrame();
        var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        AltUnityEvents.Instance.ScreenshotReady.Invoke(screenshot, size, handler);
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
    public static byte[] CompressScreenshot(byte[] screenshotSerialized)
    {

        using (var memoryStreamInput = new MemoryStream(screenshotSerialized))
        using (var memoryStreamOutout = new MemoryStream())
        {
            using (var gZipStream = new GZipStream(memoryStreamOutout, CompressionMode.Compress))
            {
                CopyTo(memoryStreamInput, gZipStream);
            }

            return memoryStreamOutout.ToArray();
        }

    }
}



