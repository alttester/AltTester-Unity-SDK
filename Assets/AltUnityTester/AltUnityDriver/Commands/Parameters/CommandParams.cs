using System;
using Altom.AltUnityDriver.Logging;
using Altom.AltUnityDriver.Notifications;
using Newtonsoft.Json;
namespace Altom.AltUnityDriver.Commands
{
    public class CommandParams
    {
        public string messageId;
        public string commandName;
        public CommandParams()
        {
            CommandAttribute cmdAttribute =
                (CommandAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(CommandAttribute));
            if (cmdAttribute == null)
                throw new Exception("No CommandAttribute found on type " + this.GetType());
            this.commandName = cmdAttribute.Name;
        }

        [JsonConstructor]
        public CommandParams(string commandName, string messageId)
        {
            this.commandName = commandName;
            this.messageId = messageId;
        }
    }

    public class CommandAttribute : Attribute
    {
        private string name;
        public CommandAttribute(string name)
        {
            this.name = name;
        }

        public string Name { get { return name; } }
    }

    public class CommandError
    {
        public string type;
        public string message;
        public string trace;
    }

    public class CommandResponse
    {
        public string messageId;
        public string commandName;
        public CommandError error;
        public String data;
        public bool isNotification;
    }

    public class BaseFindObjectsParams : CommandParams
    {
        public string path;
        public By cameraBy { get; protected set; }
        public string cameraPath { get; protected set; }
        public bool enabled { get; protected set; }

        public BaseFindObjectsParams(string path, By cameraBy, string cameraPath, bool enabled) : base()
        {
            this.path = path;
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
    }

    [Command("findObjects")]
    public class AltUnityFindObjectsParams : BaseFindObjectsParams
    {
        public AltUnityFindObjectsParams(string path, By cameraBy, string cameraPath, bool enabled) : base(path, cameraBy, cameraPath, enabled)
        {

        }
    }
    [Command("findObject")]
    public class AltUnityFindObjectParams : BaseFindObjectsParams
    {
        public AltUnityFindObjectParams(string path, By cameraBy, string cameraPath, bool enabled) : base(path, cameraBy, cameraPath, enabled)
        {

        }
    }

    [Command("findObjectsLight")]
    public class AltUnityFindObjectsLightParams : BaseFindObjectsParams
    {
        public AltUnityFindObjectsLightParams(string path, By cameraBy, string cameraPath, bool enabled) : base(path, cameraBy, cameraPath, enabled)
        {

        }
    }
    [Command("getAllLoadedScenesAndObjects")]
    public class AltUnityGetAllLoadedScenesAndObjectsParams : BaseFindObjectsParams
    {
        public AltUnityGetAllLoadedScenesAndObjectsParams(string path, By cameraBy, string cameraPath, bool enabled) : base(path, cameraBy, cameraPath, enabled)
        {

        }
    }

    [Command("getServerVersion")]
    public class AltUnityGetServerVersionParams : CommandParams
    {
        public AltUnityGetServerVersionParams() : base()
        {
        }
    }

    [Command("moveMouse")]
    public class AltUnityMoveMouseParams : CommandParams
    {
        public AltUnityVector2 coordinates;
        public float duration;
        public bool wait;

        public AltUnityMoveMouseParams(AltUnityVector2 coordinates, float duration, bool wait) : base()
        {
            this.coordinates = coordinates;
            this.duration = duration;
            this.wait = wait;
        }
    }

    [Command("multipointSwipe")]
    public class AltUnityMultipointSwipeParams : CommandParams
    {
        public AltUnityVector2[] positions;
        public float duration;
        public bool wait;

        public AltUnityMultipointSwipeParams(AltUnityVector2[] positions, float duration, bool wait) : base()
        {
            this.positions = positions;
            this.duration = duration;
            this.wait = wait;
        }
    }

    [Command("pressKeyboardKey")]
    public class AltUnityPressKeyboardKeyParams : CommandParams
    {
        public AltUnityKeyCode keyCode;
        public float power;
        public float duration;
        public bool wait;

        public AltUnityPressKeyboardKeyParams(AltUnityKeyCode keyCode, float power, float duration, bool wait) : base()
        {
            this.keyCode = keyCode;
            this.power = power;
            this.duration = duration;
            this.wait = wait;
        }
    }

    [Command("pressKeyboardKeys")]
    public class AltUnityPressKeyboardKeysParams : CommandParams
    {
        public AltUnityKeyCode[] keyCodes;
        public float power;
        public float duration;
        public bool wait;

        public AltUnityPressKeyboardKeysParams(AltUnityKeyCode[] keyCodes, float power, float duration, bool wait) : base()
        {
            this.keyCodes = keyCodes;
            this.power = power;
            this.duration = duration;
            this.wait = wait;
        }
    }


    [Command("scroll")]
    public class AltUnityScrollParams : CommandParams
    {
        public float speed;//TODO change to vector2
        public float speedHorizontal;
        public float duration;
        public bool wait;

        public AltUnityScrollParams(float speed, float duration, bool wait, float speedHorizontal = 0) : base()
        {
            this.speed = speed;
            this.duration = duration;
            this.wait = wait;
            this.speedHorizontal = speedHorizontal;
        }
    }

    [Command("swipe")]
    public class AltUnitySwipeParams : CommandParams
    {
        public AltUnityVector2 start;
        public AltUnityVector2 end;
        public float duration;
        public bool wait;

        public AltUnitySwipeParams(AltUnityVector2 start, AltUnityVector2 end, float duration, bool wait) : base()
        {
            this.start = start;
            this.end = end;
            this.duration = duration;
            this.wait = wait;
        }
    }


    [Command("tilt")]
    public class AltUnityTiltParams : CommandParams
    {
        public AltUnityVector3 acceleration;
        public float duration;
        public bool wait;

        public AltUnityTiltParams(AltUnityVector3 acceleration, float duration, bool wait) : base()
        {
            this.acceleration = acceleration;
            this.duration = duration;
            this.wait = wait;
        }
    }
    public class BaseAltUnityObjectParams : CommandParams
    {
        public AltUnityObject altUnityObject;
        public BaseAltUnityObjectParams(AltUnityObject altUnityObject) : base()
        {
            this.altUnityObject = altUnityObject;
        }
    }

    [Command("callComponentMethodForObject")]
    public class AltUnityCallComponentMethodForObjectParams : BaseAltUnityObjectParams
    {
        public string component;
        public string method;
        public string[] parameters;
        public string[] typeOfParameters;
        public string assembly;


        public AltUnityCallComponentMethodForObjectParams(AltUnityObject altUnityObject, string component, string method, string[] parameters, string[] typeOfParameters, string assembly) : base(altUnityObject)
        {
            this.component = component;
            this.method = method;
            this.parameters = parameters;
            this.typeOfParameters = typeOfParameters;
            this.assembly = assembly;
        }
    }

    [Command("getObjectComponentProperty")]
    public class AltUnityGetObjectComponentPropertyParams : BaseAltUnityObjectParams
    {
        public string component;
        public string property;
        public string assembly;
        public int maxDepth;

        public AltUnityGetObjectComponentPropertyParams(AltUnityObject altUnityObject, string component, string property, string assembly, int maxDepth) : base(altUnityObject)
        {
            this.component = component;
            this.property = property;
            this.assembly = assembly;
            this.maxDepth = maxDepth;
        }
    }
    [Command("setObjectComponentProperty")]
    public class AltUnitySetObjectComponentPropertyParams : BaseAltUnityObjectParams
    {
        public string component;
        public string property;
        public string assembly;
        public string value;

        public AltUnitySetObjectComponentPropertyParams(AltUnityObject altUnityObject, string component, string property, string assembly, string value) : base(altUnityObject)
        {
            this.component = component;
            this.property = property;
            this.assembly = assembly;
            this.value = value;
        }
    }


    [Command("dragObject")]
    public class AltUnityDragObjectParams : BaseAltUnityObjectParams
    {
        public AltUnityVector2 position;
        public AltUnityDragObjectParams(AltUnityObject altUnityObject, AltUnityVector2 position) : base(altUnityObject)
        {
            this.altUnityObject = altUnityObject;
            this.position = position;
        }
    }
    [Command("getAllComponents")]
    public class AltUnityGetAllComponentsParams : CommandParams
    {
        public int altUnityObjectId;
        public AltUnityGetAllComponentsParams(int altUnityObjectId) : base()
        {
            this.altUnityObjectId = altUnityObjectId;
        }
    }

    [Command("getAllFields")]
    public class AltUnityGetAllFieldsParams : CommandParams
    {
        public int altUnityObjectId;
        public AltUnityComponent altUnityComponent;
        public AltUnityFieldsSelections altUnityFieldsSelections;

        public AltUnityGetAllFieldsParams(int altUnityObjectId, AltUnityComponent altUnityComponent, AltUnityFieldsSelections altUnityFieldsSelections) : base()
        {
            this.altUnityObjectId = altUnityObjectId;
            this.altUnityComponent = altUnityComponent;
            this.altUnityFieldsSelections = altUnityFieldsSelections;
        }
    }
    [Command("getAllProperties")]
    public class AltUnityGetAllPropertiesParams : CommandParams
    {
        public int altUnityObjectId;
        public AltUnityComponent altUnityComponent;
        public AltUnityPropertiesSelections altUnityPropertiesSelections;

        public AltUnityGetAllPropertiesParams(int altUnityObjectId, AltUnityComponent altUnityComponent, AltUnityPropertiesSelections altUnityPropertiesSelections) : base()
        {
            this.altUnityObjectId = altUnityObjectId;
            this.altUnityComponent = altUnityComponent;
            this.altUnityPropertiesSelections = altUnityPropertiesSelections;
        }
    }
    [Command("getAllMethods")]
    public class AltUnityGetAllMethodsParams : CommandParams
    {
        public AltUnityComponent altUnityComponent;
        public AltUnityMethodSelection methodSelection;

        public AltUnityGetAllMethodsParams(AltUnityComponent altUnityComponent, AltUnityMethodSelection methodSelection) : base()
        {
            this.altUnityComponent = altUnityComponent;
            this.methodSelection = methodSelection;
        }
    }
    [Command("getText")]
    public class AltUnityGetTextParams : BaseAltUnityObjectParams
    {
        public AltUnityGetTextParams(AltUnityObject altUnityObject) : base(altUnityObject)
        {
        }
    }

    [Command("setText")]
    public class AltUnitySetTextParams : BaseAltUnityObjectParams
    {
        public string value;
        public bool submit;

        public AltUnitySetTextParams(AltUnityObject altUnityObject, string value, bool submit) : base(altUnityObject)
        {
            this.value = value;
            this.submit = submit;
        }
    }

    [Command("pointerDownFromObject")]
    public class AltUnityPointerDownFromObjectParams : BaseAltUnityObjectParams
    {
        public AltUnityPointerDownFromObjectParams(AltUnityObject altUnityObject) : base(altUnityObject)
        {
        }
    }

    [Command("pointerEnterObject")]
    public class AltUnityPointerEnterObjectParams : BaseAltUnityObjectParams
    {
        public AltUnityPointerEnterObjectParams(AltUnityObject altUnityObject) : base(altUnityObject)
        {
        }
    }
    [Command("pointerExitObject")]
    public class AltUnityPointerExitObjectParams : BaseAltUnityObjectParams
    {
        public AltUnityPointerExitObjectParams(AltUnityObject altUnityObject) : base(altUnityObject)
        {
        }
    }
    [Command("pointerUpFromObject")]
    public class AltUnityPointerUpFromObjectParams : BaseAltUnityObjectParams
    {
        public AltUnityPointerUpFromObjectParams(AltUnityObject altUnityObject) : base(altUnityObject)
        {
        }
    }


    [Command("getPNGScreenshot")]
    public class AltUnityGetPNGScreenshotParams : CommandParams
    {
        public AltUnityGetPNGScreenshotParams() : base()
        {
        }
    }

    [Command("getScreenshot")]
    public class AltUnityGetScreenshotParams : CommandParams
    {
        public AltUnityVector2 size;
        public int quality;
        public AltUnityGetScreenshotParams(AltUnityVector2 size, int quality) : base()
        {
            this.size = size;
            this.quality = quality;
        }
    }

    [Command("hightlightObjectScreenshot")]
    public class AltUnityHightlightObjectScreenshotParams : CommandParams
    {
        public int altUnityObjectId;
        public AltUnityColor color;
        public float width;
        public AltUnityVector2 size;
        public int quality;
        public AltUnityHightlightObjectScreenshotParams(int altUnityObjectId, AltUnityColor color, float width, AltUnityVector2 size, int quality) : base()
        {
            this.altUnityObjectId = altUnityObjectId;
            this.color = color;
            this.width = width;
            this.size = size;
            this.quality = quality;
        }
    }

    [Command("hightlightObjectFromCoordinatesScreenshot")]
    public class AltUnityHightlightObjectFromCoordinatesScreenshotParams : CommandParams
    {
        public AltUnityVector2 coordinates;
        public AltUnityColor color;
        public float width;
        public AltUnityVector2 size;
        public int quality;
        public AltUnityHightlightObjectFromCoordinatesScreenshotParams(AltUnityVector2 coordinates, AltUnityColor color, float width, AltUnityVector2 size, int quality) : base()
        {
            this.coordinates = coordinates;
            this.color = color;
            this.width = width;
            this.size = size;
            this.quality = quality;
        }
    }

    [Command("deleteKeyPlayerPref")]
    public class AltUnityDeleteKeyPlayerPrefParams : CommandParams
    {
        public string keyName;
        public AltUnityDeleteKeyPlayerPrefParams(string keyName) : base()
        {
            this.keyName = keyName;
        }
    }
    [Command("getKeyPlayerPref")]
    public class AltUnityGetKeyPlayerPrefParams : CommandParams
    {
        public string keyName;
        public PlayerPrefKeyType keyType;
        public AltUnityGetKeyPlayerPrefParams() : base() { }
        public AltUnityGetKeyPlayerPrefParams(string keyName, PlayerPrefKeyType keyType) : base()
        {
            this.keyName = keyName;
            this.keyType = keyType;
        }
    }

    [Command("setKeyPlayerPref")]
    public class AltUnitySetKeyPlayerPrefParams : CommandParams
    {
        public string keyName;
        public PlayerPrefKeyType keyType;
        public string stringValue;
        public float floatValue;
        public int intValue;

        public AltUnitySetKeyPlayerPrefParams() : base() { }

        public AltUnitySetKeyPlayerPrefParams(string keyName, int value) : base()
        {
            this.keyName = keyName;
            this.intValue = value;
            keyType = PlayerPrefKeyType.Int;
        }
        public AltUnitySetKeyPlayerPrefParams(string keyName, float value) : base()
        {
            this.keyName = keyName;
            this.floatValue = value;
            keyType = PlayerPrefKeyType.Float;
        }
        public AltUnitySetKeyPlayerPrefParams(string keyName, string value) : base()
        {
            this.keyName = keyName;
            this.stringValue = value;
            keyType = PlayerPrefKeyType.String;
        }
    }

    [Command("deletePlayerPref")]
    public class AltUnityDeletePlayerPrefParams : CommandParams
    {
        public AltUnityDeletePlayerPrefParams() : base()
        {
        }
    }

    [Command("getAllActiveCameras")]
    public class AltUnityGetAllActiveCamerasParams : CommandParams
    {
        public AltUnityGetAllActiveCamerasParams() : base()
        {
        }
    }
    [Command("getAllCameras")]
    public class AltUnityGetAllCamerasParams : CommandParams
    {

    }

    [Command("getAllLoadedScenes")]
    public class AltUnityGetAllLoadedScenesParams : CommandParams
    {

    }
    [Command("getAllScenes")]
    public class AltUnityGetAllScenesParams : CommandParams
    {

    }
    [Command("getCurrentScene")]
    public class AltUnityGetCurrentSceneParams : CommandParams
    {

    }
    [Command("loadScene")]
    public class AltUnityLoadSceneParams : CommandParams
    {
        public string sceneName;
        public bool loadSingle;

        public AltUnityLoadSceneParams(string sceneName, bool loadSingle) : base()
        {
            this.sceneName = sceneName;
            this.loadSingle = loadSingle;
        }

    }

    [Command("unloadScene")]
    public class AltUnityUnloadSceneParams : CommandParams
    {
        public string sceneName;

        public AltUnityUnloadSceneParams(string sceneName) : base()
        {
            this.sceneName = sceneName;
        }

    }
    [Command("getTimeScale")]
    public class AltUnityGetTimeScaleParams : CommandParams
    {

    }
    [Command("setTimeScale")]
    public class AltUnitySetTimeScaleParams : CommandParams
    {
        public float timeScale;
        public AltUnitySetTimeScaleParams(float timeScale) : base()
        {
            this.timeScale = timeScale;
        }
    }
    [Command("setServerLogging")]
    public class AltUnitySetServerLoggingParams : CommandParams
    {
        public AltUnityLogger logger;
        public AltUnityLogLevel logLevel;

        public AltUnitySetServerLoggingParams(AltUnityLogger logger, AltUnityLogLevel logLevel) : base()
        {
            this.logger = logger;
            this.logLevel = logLevel;
        }
    }

    [Command("tapElement")]
    public class AltUnityTapElementParams : BaseAltUnityObjectParams
    {
        public int count;
        public float interval;
        public bool wait;
        public AltUnityTapElementParams(AltUnityObject altUnityObject, int count, float interval, bool wait) : base(altUnityObject)
        {
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }
    }

    [Command("clickElement")]
    public class AltUnityClickElementParams : BaseAltUnityObjectParams
    {
        public int count;
        public float interval;
        public bool wait;
        public AltUnityClickElementParams(AltUnityObject altUnityObject, int count, float interval, bool wait) : base(altUnityObject)
        {
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }
    }

    [Command("clickCoordinates")]
    public class AltUnityClickCoordinatesParams : CommandParams
    {
        public AltUnityVector2 coordinates;
        public int count;
        public float interval;
        public bool wait;
        public AltUnityClickCoordinatesParams(AltUnityVector2 coordinates, int count, float interval, bool wait)
        {
            this.coordinates = coordinates;
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }
    }
    [Command("tapCoordinates")]
    public class AltUnityTapCoordinatesParams : CommandParams
    {
        public AltUnityVector2 coordinates;
        public int count;
        public float interval;
        public bool wait;
        public AltUnityTapCoordinatesParams(AltUnityVector2 coordinates, int count, float interval, bool wait)
        {
            this.coordinates = coordinates;
            this.count = count;
            this.interval = interval;
            this.wait = wait;
        }
    }

    [Command("keyUp")]
    public class AltUnityKeyUpParams : CommandParams
    {
        public AltUnityKeyCode keyCode;

        public AltUnityKeyUpParams(AltUnityKeyCode keyCode)
        {
            this.keyCode = keyCode;
        }
    }

    [Command("keysUp")]
    public class AltUnityKeysUpParams : CommandParams
    {
        public AltUnityKeyCode[] keyCodes;

        public AltUnityKeysUpParams(AltUnityKeyCode[] keyCodes)
        {
            this.keyCodes = keyCodes;
        }
    }

    [Command("keyDown")]
    public class AltUnityKeyDownParams : CommandParams
    {
        public AltUnityKeyCode keyCode;
        public float power;

        public AltUnityKeyDownParams(AltUnityKeyCode keyCode, float power)
        {
            this.keyCode = keyCode;
            this.power = power;
        }
    }

    [Command("keysDown")]
    public class AltUnityKeysDownParams : CommandParams
    {
        public AltUnityKeyCode[] keyCodes;
        public float power;

        public AltUnityKeysDownParams(AltUnityKeyCode[] keyCodes, float power)
        {
            this.keyCodes = keyCodes;
            this.power = power;
        }
    }

    [Command("beginTouch")]
    public class AltUnityBeginTouchParams : CommandParams
    {
        public AltUnityVector2 coordinates;
        public AltUnityBeginTouchParams(AltUnityVector2 coordinates)
        {
            this.coordinates = coordinates;
        }
    }

    [Command("moveTouch")]
    public class AltUnityMoveTouchParams : CommandParams
    {
        public int fingerId;
        public AltUnityVector2 coordinates;
        public AltUnityMoveTouchParams(int fingerId, AltUnityVector2 coordinates)
        {
            this.coordinates = coordinates;
            this.fingerId = fingerId;
        }
    }

    [Command("endTouch")]
    public class AltUnityEndTouchParams : CommandParams
    {
        public int fingerId;
        public AltUnityEndTouchParams(int fingerId)
        {
            this.fingerId = fingerId;
        }
    }
    [Command("activateNotification")]
    public class ActivateNotification : CommandParams
    {
        public NotificationType NotificationType;

        public ActivateNotification(NotificationType notificationType)
        {
            NotificationType = notificationType;
        }
    }
    [Command("deactivateNotification")]
    public class DeactivateNotification : CommandParams
    {
        public NotificationType NotificationType;

        public DeactivateNotification(NotificationType notificationType)
        {
            NotificationType = notificationType;
        }
    }

    [Command("findObjectAtCoordinates")]
    public class AltUnityFindObjectAtCoordinatesParams : CommandParams
    {
        public AltUnityVector2 coordinates;
        public AltUnityFindObjectAtCoordinatesParams(AltUnityVector2 coordinates)
        {
            this.coordinates = coordinates;
        }
    }
}