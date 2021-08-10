using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityDriver.Logging;
using Altom.Server.Logging;
using Assets.AltUnityTester.AltUnityServer;
using Assets.AltUnityTester.AltUnityServer.AltSocket;
using Assets.AltUnityTester.AltUnityServer.Commands;
using Newtonsoft.Json;
using NLog;

public class AltUnityRunner : UnityEngine.MonoBehaviour, AltIClientSocketHandlerDelegate
{
    private static readonly Logger logger = ServerLogManager.Instance.GetCurrentClassLogger();

    public static readonly string VERSION = "1.6.6";
    public static AltUnityRunner _altUnityRunner;
    public static AltResponseQueue _responseQueue;

    public UnityEngine.GameObject AltUnityPopUp;
    public UnityEngine.UI.Image AltUnityIcon;
    public UnityEngine.UI.Text AltUnityPopUpText;
    public bool AltUnityIconPressed = false;
    [UnityEngine.Space]
    public bool showPopUp;
    public int SocketPortNumber = 13000;
    public int MaxLogLength = 100;
    public bool RunOnlyInDebugMode = true;
    public UnityEngine.Shader outlineShader;
    public UnityEngine.GameObject panelHightlightPrefab;
    public string requestSeparatorString = ";";
    public string requestEndingString = "&";

    [UnityEngine.SerializeField]
    public UnityEngine.GameObject AltUnityPopUpCanvas = null;
    private AltSocketServer socketServer;
    [UnityEngine.Space]
    [UnityEngine.SerializeField]
    private bool _showInputs = false;
    [UnityEngine.SerializeField]
    public AltUnityInputsVisualiser _inputsVisualiser = null;

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

    protected void Awake()
    {
        if (_altUnityRunner != null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (RunOnlyInDebugMode && !UnityEngine.Debug.isDebugBuild)
        {
            logger.Warn("AltUnityTester runs only on Debug build");
            Destroy(this.gameObject);
            return;
        }

        ServerLogManager.SetupAltUnityServerLogging(new Dictionary<AltUnityLogger, AltUnityLogLevel> { { AltUnityLogger.File, AltUnityLogLevel.Debug }, { AltUnityLogger.Unity, AltUnityLogLevel.Debug } });

        _altUnityRunner = this;
        DontDestroyOnLoad(this);
    }
    protected void Start()
    {
        AltClientSocketHandler.MaxLogLength = MaxLogLength;
        StartSocketServer();
        logger.Debug("AltUnity Server started");
        _responseQueue = new AltResponseQueue();

        if (showPopUp == false)
        {
            AltUnityPopUpCanvas.SetActive(false);
        }
        else
        {
            AltUnityPopUpCanvas.SetActive(true);
        }
    }

    protected void Update()
    {
#if UNITY_EDITOR
        if (socketServer == null)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
#endif
        if (!AltUnityIconPressed)
        {
            if (socketServer.ClientCount != 0)
            {
                AltUnityPopUp.SetActive(false);
            }
            else
            {
                AltUnityPopUp.SetActive(true);
            }
        }
        if (!socketServer.IsServerStopped())
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
    protected void OnApplicationQuit()
    {
        CleanUp();
    }

    #endregion
    #region public methods
    public void CleanUp()
    {
        logger.Debug("Cleaning up socket server");
        if (socketServer != null)
            socketServer.Cleanup();
    }

    public void StartSocketServer()
    {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;

        System.Text.Encoding encoding = System.Text.Encoding.UTF8;

        socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, SocketPortNumber, maxClients, requestEndingString, encoding);

        try
        {
            socketServer.StartListeningForConnections();
            AltUnityPopUpText.text = "Waiting for connection" + System.Environment.NewLine + "on port " + socketServer.PortNumber + "...";
            logger.Info(string.Format(
                "AltUnity Server is listening on {0}:{1}",
                socketServer.LocalEndPoint.Address, socketServer.PortNumber));
        }
        catch (SocketException ex)
        {
            if (ex.Message.Contains("Only one usage of each socket address"))
            {
                AltUnityPopUpText.text = "Cannot start AltUnity Server. Another process is listening on port " + SocketPortNumber;
                logger.Info("Cannot start AltUnity Server. Another process is listening on port" + SocketPortNumber);
            }
            else
            {
                AltUnityPopUpText.text = "An error occured while starting AltUnity Server.";
                logger.Error(ex);
            }
        }
    }

    public AltUnityObject GameObjectToAltUnityObject(UnityEngine.GameObject altGameObject, UnityEngine.Camera camera = null)
    {
        UnityEngine.Vector3 position;

        int cameraId;
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

        var altObject = new AltUnityObject(
            name: altGameObject.name,
            id: altGameObject.GetInstanceID(),
            x: Convert.ToInt32(UnityEngine.Mathf.Round(position.x)),
            y: Convert.ToInt32(UnityEngine.Mathf.Round(position.y)),
            z: Convert.ToInt32(UnityEngine.Mathf.Round(position.z)),//if z is negative object is behind the camera
            mobileY: Convert.ToInt32(UnityEngine.Mathf.Round(UnityEngine.Screen.height - position.y)),
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
        int transformParentId = altGameObject.transform.parent == null ? 0 : altGameObject.transform.parent.GetInstanceID();
        AltUnityObjectLight altObject = new AltUnityObjectLight(
            name: altGameObject.name,
            id: altGameObject.GetInstanceID(),
            enabled: altGameObject.activeSelf,
            idCamera: 0,
            transformId: altGameObject.transform.GetInstanceID(),
            transformParentId: transformParentId,
            parentId: transformParentId);

        return altObject;
    }

    public void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message)
    {
        logger.Debug("command received: " + message);
        string[] parameters = message.Split(new string[] { requestSeparatorString }, StringSplitOptions.None);

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
                case "beginTouch":
                    command = new AltUnityBeginTouchCommand(parameters);
                    break;
                case "moveTouch":
                    command = new AltUnityMoveTouchCommand(parameters);
                    break;
                case "endTouch":
                    command = new AltUnityEndTouchCommand(parameters);
                    break;
                case "tapElement":
                    command = new AltUnityTapElementCommand(handler, parameters);
                    break;
                case "clickElement":
                    command = new AltUnityClickElementCommand(handler, parameters);
                    break;

                case "tapCoordinates":
                    command = new AltUnityTapCoordinatesCommand(handler, parameters);
                    break;
                case "clickCoordinates":
                    command = new AltUnityClickCoordinatesCommand(handler, parameters);
                    break;

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
                    socketServer.StartListeningForConnections();
                    logger.Debug("Socket connection closed!");
                    break;
                case "clickEvent":
                    command = new AltUnityClickEventCommand(parameters);
                    break;
                case "tapScreen":
                    command = new AltUnityTapAtCoordinatesCommand(parameters);
                    break;
                case "tapCustom":
                    command = new AltUnityTapAtCoordinatesCustomCommand(parameters);
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
                case "unloadScene":
                    command = new AltUnityUnloadSceneCommand(handler, parameters);
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
                    command = new AltUnityGetAllCamerasCommand(false, parameters);
                    break;
                case "getAllActiveCameras":
                    command = new AltUnityGetAllCamerasCommand(true, parameters);
                    break;
                case "getAllLoadedScenes":
                    command = new AltUnityGetAllLoadedScenesCommand(parameters);
                    break;
                case "getAllLoadedScenesAndObjects":
                    command = new AltUnityGetAllLoadedScenesAndObjectsCommand(parameters);
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
                case "setServerLogging":
                    command = new AltUnitySetServerLoggingCommand(parameters);
                    break;
                case "keyDown":
                    command = new AltUnityKeyDownCommand(parameters);
                    break;
                case "keyUp":
                    command = new AltUnityKeyUpCommand(parameters);
                    break;
                default:
                    command = new AltUnityUnknowStringCommand(parameters);
                    break;
            }
        }
        catch (JsonException ex)
        {
            command = new AltUnityErrorCommand(AltUnityErrors.errorCouldNotParseJsonString, ex, parameters);
        }
        catch (InvalidParametersOnDriverCommandException ex)
        {
            command = new AltUnityErrorCommand(AltUnityErrors.errorInvalidParametersOnDriverCommand, ex, parameters);
        }
        if (command != null)
        {
            _responseQueue.ScheduleResponse(delegate
           {
               var result = command.ExecuteHandleErrors(command.Execute);

               var logs = command.GetLogs();
               if (!string.IsNullOrEmpty(logs)) logs += "\n";
               logs += result.Item2;
               handler.SendResponse(command.MessageId, command.CommandName, result.Item1, logs);
           });
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
        socketServer.Cleanup();
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

    public System.Collections.IEnumerator RunActionAfterEndOfFrame(Action action)
    {
        yield return new UnityEngine.WaitForEndOfFrame();
        action();
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
    #endregion
    #region private methods
    private UnityEngine.Vector3 getObjectScreenPosition(UnityEngine.GameObject gameObject, UnityEngine.Camera camera)
    {
        var selectedCamera = camera;
        var position = gameObject.transform.position;
        UnityEngine.Canvas canvas = gameObject.GetComponentInParent<UnityEngine.Canvas>();
        if (canvas != null)
        {
            if (canvas.renderMode != UnityEngine.RenderMode.ScreenSpaceOverlay)
            {
                if (gameObject.GetComponent<UnityEngine.RectTransform>() != null)
                {
                    UnityEngine.Vector3[] vector3S = new UnityEngine.Vector3[4];
                    gameObject.GetComponent<UnityEngine.RectTransform>().GetWorldCorners(vector3S);
                    position = new UnityEngine.Vector3((vector3S[0].x + vector3S[2].x) / 2, (vector3S[0].y + vector3S[2].y) / 2, (vector3S[0].z + vector3S[2].z) / 2);
                }
                if (canvas.worldCamera != null)
                {
                    selectedCamera = canvas.worldCamera;
                }
                return selectedCamera.WorldToScreenPoint(position);
            }

            if (gameObject.GetComponent<UnityEngine.RectTransform>() != null)
            {
                return gameObject.GetComponent<UnityEngine.RectTransform>().position;
            }
            return camera.WorldToScreenPoint(gameObject.transform.position);
        }

        var collider = gameObject.GetComponent<UnityEngine.Collider>();
        if (collider != null)
        {
            position = collider.bounds.center;
        }

        return camera.WorldToScreenPoint(position);
    }
    ///<summary>
    /// Iterate through all cameras until finds one that sees the object.
    /// If no camera sees the object return the position from the last camera
    ///</summary>
    public int findCameraThatSeesObject(UnityEngine.GameObject gameObject, out UnityEngine.Vector3 position)
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
