using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Assets.AltUnityTester.AltUnityServer.Commands;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Sockets;

public class AltUnityRunner : UnityEngine.MonoBehaviour, AltIClientSocketHandlerDelegate
{

    public static readonly string VERSION = "1.6.2";
    public static AltUnityRunner _altUnityRunner;
    public static System.IO.StreamWriter ServerLogger;
    public static AltResponseQueue _responseQueue;

    public UnityEngine.GameObject AltUnityPopUp;
    public UnityEngine.UI.Image AltUnityIcon;
    public UnityEngine.UI.Text AltUnityPopUpText;
    public bool AltUnityIconPressed = false;
    public JsonSerializerSettings _jsonSettings;
    [UnityEngine.Space]
    public bool showPopUp;
    public bool destroyHightlight = false;
    public int SocketPortNumber = 13000;
    public bool RunOnlyInDebugMode = true;
    public UnityEngine.Shader outlineShader;
    public UnityEngine.GameObject panelHightlightPrefab;
    public string requestSeparatorString = ";";
    public string requestEndingString = "&";

    [UnityEngine.SerializeField]
    private UnityEngine.GameObject AltUnityPopUpCanvas = null;
    private AltSocketServer _socketServer;
    private string myPathFile;
    [UnityEngine.Space]
    [UnityEngine.SerializeField]
    private bool _showInputs = false;
    [UnityEngine.SerializeField]
    private AltUnityInputsVisualiser _inputsVisualiser = null;

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
        if (_altUnityRunner != null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (RunOnlyInDebugMode && !UnityEngine.Debug.isDebugBuild)
        {
            UnityEngine.Debug.LogWarning("AltUnityTester runs only on Debug build");
            Destroy(this.gameObject);
            return;
        }

        _altUnityRunner = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        _jsonSettings = new JsonSerializerSettings();
        _jsonSettings.NullValueHandling = NullValueHandling.Ignore;
        StartSocketServer();
        UnityEngine.Debug.Log("AltUnity Driver started");
        _responseQueue = new AltResponseQueue();

        myPathFile = UnityEngine.Application.persistentDataPath + "/AltUnityServerLog.txt";
        UnityEngine.Debug.Log("AltUnity Server logs path: " + myPathFile);
        ServerLogger = new System.IO.StreamWriter(myPathFile, false);//To not create a massive logfile the logfile will have only the last run.
        if (showPopUp == false)
        {
            AltUnityPopUpCanvas.SetActive(false);
        }
        else
        {
            AltUnityPopUpCanvas.SetActive(true);
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (_socketServer == null)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif
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
            AltUnityPopUpText.text = "Server stopped working." + System.Environment.NewLine + " Please restart the server";
        }
        _responseQueue.Cycle();
    }
    void OnApplicationQuit()
    {
        CleanUp();
        if (ServerLogger != null)
            ServerLogger.Close();
    }

    #endregion
    public void CleanUp()
    {
        UnityEngine.Debug.Log("Cleaning up socket server");
        if (_socketServer != null)
            _socketServer.Cleanup();
    }

    public void StartSocketServer()
    {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;

        System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        _socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, SocketPortNumber, maxClients, requestEndingString, encoding);

        try
        {
            _socketServer.StartListeningForConnections();
            AltUnityPopUpText.text = "Waiting for connection" + System.Environment.NewLine + "on port " + _socketServer.PortNumber + "...";
            UnityEngine.Debug.Log(string.Format(
                "AltUnity Server at {0} on port {1}",
                _socketServer.LocalEndPoint.Address, _socketServer.PortNumber));
        }
        catch (SocketException ex)
        {
            if (ex.Message.Contains("Only one usage of each socket address"))
            {
                AltUnityPopUpText.text = "Cannot start AltUnity Server. Another process is listening on port " + SocketPortNumber;
            }
            else
            {
                UnityEngine.Debug.LogError(ex);
                AltUnityPopUpText.text = "An error occured while starting AltUnity Server.";
            }
        }
    }


    public AltUnityObject GameObjectToAltUnityObject(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
    {
        UnityEngine.Vector3 position;

        int cameraId = -1;
        //if no camera is given it will iterate through all cameras until  found one that sees the object if no camera sees the object it will return the position from the last camera
        //if there is no camera in the scene it will return as scren position x:-1 y=-1, z=-1 and cameraId=-1
        try
        {
            if (camera == null)
            {
                cameraId = findCameraThatSeesObject(altGameObject, out position);
            }
            else
            {
                position = getObjectScreenPosition(altGameObject, camera);
                cameraId = camera.GetInstanceID();
            }
        }
        catch (Exception)
        {
            position = UnityEngine.Vector3.one * -1;
            cameraId = -1;
        }

        int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();

        AltUnityObject altObject = new AltUnityObject(
            name: altGameObject.name,
            id: altGameObject.GetInstanceID(),
            x: System.Convert.ToInt32(UnityEngine.Mathf.Round(position.x)),
            y: System.Convert.ToInt32(UnityEngine.Mathf.Round(position.y)),
            z: System.Convert.ToInt32(UnityEngine.Mathf.Round(position.z)),//if z is negative object is behind the camera
            mobileY: System.Convert.ToInt32(UnityEngine.Mathf.Round(UnityEngine.Screen.height - position.y)),
            type: "",
            enabled: altGameObject.activeSelf,
            worldX: altGameObject.transform.position.x,
            worldY: altGameObject.transform.position.y,
            worldZ: altGameObject.transform.position.z,
            idCamera: cameraId,
            transformId: altGameObject.transform.GetInstanceID(),
            transformParentId: transformParentId,
            parentId: transformParentId);
        return altObject;
    }

    public AltUnityObjectLight GameObjectToAltUnityObjectLight(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
    {
        int cameraId = -1;
        UnityEngine.Vector3 position;
        //if no camera is given it will iterate through all cameras until  found one that sees the object if no camera sees the object it will return the position from the last camera
        //if there is no camera end this is Unity UI element, it return position by root Canvas and cameraId=-1
        //if there is no camera in the scene it will return as screen position x:-1 y=-1, z=-1 and cameraId=-1
        try
        {
            if (camera == null)
            {
                cameraId = findCameraThatSeesObject(altGameObject, out position);
            }
            else
            {
                position = getObjectScreenPosition(altGameObject, camera);
                cameraId = camera.GetInstanceID();
            }
        }
        catch (Exception)
        {
            position = UnityEngine.Vector3.one * -1;
            cameraId = -1;
        }

        int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();

        AltUnityObjectLight altObject = new AltUnityObjectLight(
            name: altGameObject.name,
            id: altGameObject.GetInstanceID(),
            enabled: altGameObject.activeSelf,
            idCamera: cameraId,
            transformId: altGameObject.transform.GetInstanceID(),
            transformParentId: transformParentId,
            parentId: transformParentId);

        return altObject;
    }

    public void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message)
    {
        string[] parameters = message.Split(new string[] { requestSeparatorString }, System.StringSplitOptions.None);

        AltUnityCommand command = null;
        try
        {
            if (parameters[0] == "getServerVersion")
            {
                var versionCommand = new AltUnityGetServerVersionCommandBackwardsCompatible();
                versionCommand.SendResponse(handler);
                return;
            }
            switch (parameters[1])
            {
                case "tapObject":
                    command = new AltUnityTapCommand(parameters);
                    break;
                case "getCurrentScene":
                    command = new AltUnityGetCurrentSceneCommand(parameters);
                    break;
                case "getObjectComponentProperty":
                    command = new AltUnityGetComponentPropertyCommand(parameters);
                    break;
                case "setObjectComponentProperty":
                    command = new AltUnitySetObjectComponentPropertyCommand(parameters);
                    break;
                case "callComponentMethodForObject":
                    command = new AltUnityCallComponentMethodForObjectCommand(parameters);
                    break;
                case "closeConnection":
                    UnityEngine.Debug.Log("Socket connection closed!");
                    _socketServer.StartListeningForConnections();
                    break;
                case "clickEvent":
                    command = new AltUnityClickEventCommand(parameters);
                    break;
                case "tapScreen":
                    command = new AltUnityClickOnScreenAtXyCommand(parameters);
                    break;
                case "tapCustom":
                    command = new AltUnityClickOnScreenCustom(parameters);
                    break;
                case "dragObject":
                    command = new AltUnityDragObjectCommand(parameters);
                    break;
                case "dropObject":
                    command = new AltUnityDropObjectCommand(parameters);
                    break;
                case "pointerUpFromObject":
                    command = new AltUnityPointerUpFromObjectCommand(parameters);
                    break;
                case "pointerDownFromObject":
                    command = new AltUnityPointerDownFromObjectCommand(parameters);
                    break;
                case "pointerEnterObject":
                    command = new AltUnityPointerEnterObjectCommand(parameters);
                    break;
                case "pointerExitObject":
                    command = new AltUnityPointerExitObjectCommand(parameters);
                    break;
                case "tilt":
                    command = new AltUnityTiltCommand(parameters);
                    break;
                case "multipointSwipe":
                    command = new AltUnitySetMultipointSwipeCommand(parameters);
                    break;
                case "multipointSwipeChain":
                    command = new AltUnitySetMultipointSwipeChainCommand(parameters);
                    break;
                case "loadScene":
                    command = new AltUnityLoadSceneCommand(handler, parameters);
                    break;
                case "setTimeScale":
                    command = new AltUnitySetTimeScaleCommand(parameters);
                    break;
                case "getTimeScale":
                    command = new AltUnityGetTimeScaleCommand(parameters);
                    break;
                case "deletePlayerPref":
                    command = new AltUnityDeletePlayerPrefCommand(parameters);
                    break;
                case "deleteKeyPlayerPref":
                    command = new AltUnityDeleteKeyPlayerPrefCommand(parameters);
                    break;
                case "setKeyPlayerPref":
                    command = new AltUnitySetKeyPlayerPrefCommand(parameters);
                    break;
                case "getKeyPlayerPref":
                    command = new AltUnityGetKeyPlayerPrefCommand(parameters);
                    break;
                case "actionFinished":
                    command = new AltUnityActionFinishedCommand(parameters);
                    break;
                case "getAllComponents":
                    command = new AltUnityGetAllComponentsCommand(parameters);
                    break;
                case "getAllFields":
                    command = new AltUnityGetAllFieldsCommand(parameters);
                    break;
                case "getAllProperties":
                    command = new AltUnityGetAllPropertiesCommand(parameters);
                    break;
                case "getAllMethods":
                    command = new AltUnityGetAllMethodsCommand(parameters);
                    break;
                case "getAllScenes":
                    command = new AltUnityGetAllScenesCommand(parameters);
                    break;
                case "getAllCameras":
                    command = new AltUnityGetAllCamerasCommand(parameters);
                    break;
                case "getAllLoadedScenes":
                    command = new AltUnityGetAllLoadedScenesCommand(parameters);
                    break;
                case "getScreenshot":
                    command = new AltUnityGetScreenshotCommand(handler, parameters);
                    break;

                case "hightlightObjectScreenshot":
                    command = new AltUnityHighlightSelectedObjectCommand(handler, parameters);
                    break;
                case "hightlightObjectFromCoordinatesScreenshot":
                    command = new AltUnityHightlightObjectFromCoordinatesCommand(handler, parameters);
                    break;
                case "pressKeyboardKey":
                    command = new AltUnityHoldButtonCommand(parameters);
                    break;
                case "moveMouse":
                    command = new AltUnityMoveMouseCommand(parameters);
                    break;
                case "scrollMouse":
                    command = new AltUnityScrollMouseCommand(parameters);

                    break;
                case "findObject":
                    command = new AltUnityFindObjectCommand(parameters);
                    break;
                case "findObjects":
                    command = new AltUnityFindObjectsCommand(parameters);
                    break;
                case "findObjectsLight":
                    command = new AltUnityFindObjectsLightCommand(parameters);
                    break;
                case "findActiveObjectByName":
                    command = new AltUnityFindActiveObjectsByNameCommand(parameters);
                    break;
                case "enableLogging":
                    command = new AltUnityEnableLoggingCommand(parameters);
                    break;
                case "getText":
                    command = new AltUnityGetTextCommand(parameters);
                    break;
                case "setText":
                    command = new AltUnitySetTextCommand(parameters);
                    break;
                case "getPNGScreenshot":
                    command = new AltUnityGetScreenshotPNGCommand(handler, parameters);
                    break;
                case "getServerVersion":
                    command = new AltUnityGetServerVersionCommand(parameters);
                    break;
                default:
                    command = new AltUnityUnknowStringCommand(parameters);
                    break;
            }
        }
        catch (JsonException ex)
        {
            command = new AltUnityCouldNotParseJsonStringCommand(parameters);
            ((AltUnityCouldNotParseJsonStringCommand)command).LogMessage(ex.Message);
        }
        catch (InvalidParametersOnDriverCommandException ex)
        {
            command = new AltUnityInvalidParametersOnDriverCommandCommand(parameters);
            ((AltUnityInvalidParametersOnDriverCommandCommand)command).LogMessage(ex.Message);
        }
        if (command != null)
        {
            command.SendResponse(handler);
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
        throw new NotFoundException("Object not found");
    }

    public static UnityEngine.GameObject GetGameObject(int objectId)
    {
        foreach (UnityEngine.GameObject gameObject in UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>())
        {
            if (gameObject.GetInstanceID() == objectId)
                return gameObject;
        }
        throw new NotFoundException("Object not found");
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

    public System.Collections.IEnumerator HighLightSelectedObjectCorutine(UnityEngine.GameObject gameObject, UnityEngine.Color color, float width, AltUnityGetScreenshotCommand getScreenshotCommand)
    {
        destroyHightlight = false;
        UnityEngine.Renderer renderer = gameObject.GetComponent<UnityEngine.Renderer>();
        if (renderer != null)
        {
            var originalMaterials = renderer.materials.ToArray();
            renderer.materials = new UnityEngine.Material[renderer.materials.Length];
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                renderer.materials[i] = new UnityEngine.Material(originalMaterials[i]);
                renderer.materials[i].shader = outlineShader;
                renderer.materials[i].SetColor("_OutlineColor", color);
                renderer.materials[i].SetFloat("_OutlineWidth", width);
            }
            yield return null;
            getScreenshotCommand.Execute();
            yield return null;
            renderer.materials = originalMaterials;
        }
        else
        {
            var rectTransform = gameObject.GetComponent<UnityEngine.RectTransform>();
            if (rectTransform != null)
            {
                var panelHighlight = Instantiate(panelHightlightPrefab, rectTransform);
                panelHighlight.GetComponent<UnityEngine.UI.Image>().color = color;
                yield return null;
                getScreenshotCommand.Execute();
                while (!destroyHightlight)
                    yield return null;
                Destroy(panelHighlight);
                destroyHightlight = false;
            }
            else
            {
                getScreenshotCommand.Execute();
            }
        }
    }

    public System.Collections.IEnumerator TakeTexturedScreenshot(AltClientSocketHandler handler, AltUnityScreenshotReadyCommand screenshotReadyCommand)
    {
        yield return new UnityEngine.WaitForEndOfFrame();


        var response = screenshotReadyCommand.Execute();

        handler.SendScreenshotResponse(screenshotReadyCommand, response);
    }
    public System.Collections.IEnumerator TakeScreenshot(AltUnityCommand command, AltClientSocketHandler handler)
    {
        yield return new UnityEngine.WaitForEndOfFrame();
        var screenshot = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();
        var bytesPNG = UnityEngine.ImageConversion.EncodeToPNG(screenshot);
        var pngAsString = Convert.ToBase64String(bytesPNG);
        UnityEngine.GameObject.DestroyImmediate(screenshot);
        handler.SendScreenshotResponse(command, pngAsString);
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

    #region private methods
    private UnityEngine.Vector3 getObjectScreenPosition(UnityEngine.GameObject gameObject, UnityEngine.Camera camera)
    {
        UnityEngine.Canvas canvas = gameObject.GetComponentInParent<UnityEngine.Canvas>();
        if (canvas != null)
        {
            if (canvas.renderMode != UnityEngine.RenderMode.ScreenSpaceOverlay)
            {
                if (gameObject.GetComponent<UnityEngine.RectTransform>() == null)
                {
                    if (canvas.worldCamera != null)
                    {
                        return canvas.worldCamera.WorldToScreenPoint(gameObject.transform.position);
                    }
                }
                else
                {
                    UnityEngine.Vector3[] vector3S = new UnityEngine.Vector3[4];
                    gameObject.GetComponent<UnityEngine.RectTransform>().GetWorldCorners(vector3S);
                    var center = new UnityEngine.Vector3((vector3S[0].x + vector3S[2].x) / 2, (vector3S[0].y + vector3S[2].y) / 2, (vector3S[0].z + vector3S[2].z) / 2);
                    if (canvas.worldCamera != null)
                    {
                        return canvas.worldCamera.WorldToScreenPoint(center);
                    }
                }
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
    ///<summary>
    /// Iterate through all cameras until finds one that sees the object.
    /// If no camera sees the object return the position from the last camera
    ///</summary>
    private int findCameraThatSeesObject(UnityEngine.GameObject gameObject, out UnityEngine.Vector3 position)
    {
        position = UnityEngine.Vector3.one * -1;
        int cameraId = -1;
        if (UnityEngine.Camera.allCamerasCount == 0)
        {
            var rectTransform = gameObject.GetComponent<UnityEngine.RectTransform>();
            if (rectTransform != null)
            {
                var canvas = rectTransform.GetComponentInParent<UnityEngine.Canvas>();
                if (canvas != null)
                    position = UnityEngine.RectTransformUtility.PixelAdjustPoint(rectTransform.position, rectTransform, canvas.rootCanvas);
            }
            return cameraId;
        }
        foreach (var camera1 in UnityEngine.Camera.allCameras)
        {
            position = getObjectScreenPosition(gameObject, camera1);
            cameraId = camera1.GetInstanceID();
            if (position.x > 0 &&
                position.y > 0 &&
                position.x < UnityEngine.Screen.width &&
                position.y < UnityEngine.Screen.height &&
                position.z >= 0)//Check if camera sees the object
            {
                break;
            }
        }
        return cameraId;
    }
    #endregion
}
