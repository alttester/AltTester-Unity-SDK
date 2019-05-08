
public class FindObjectByNameCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectWhereNameContainsCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectByComponentCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }

public class FindObjectsByNameCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectsWhereNameContainsCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectsByComponentCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }

public class GetCurrentSceneCommand : UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }
public class GetAllObjectsCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }

public class ClickEventCommand : UnityEngine.Events.UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class TapScreenCommand : UnityEngine.Events.UnityEvent<string, string, AltClientSocketHandler> { }

public class GetComponentPropertyCommand : UnityEngine.Events.UnityEvent<string, string, AltClientSocketHandler> { }
public class SetComponentPropertyCommand : UnityEngine.Events.UnityEvent<string, string, string, AltClientSocketHandler> { }
public class CallComponentMethodCommand : UnityEngine.Events.UnityEvent<string, string, AltClientSocketHandler> { }
public class GetTextCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }

public class CloseConnectionCommand : UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }
public class UnknownStringCommand : UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }

public class SetStationaryTouchCommand : UnityEngine.Events.UnityEvent<UnityEngine.Touch, string, AltClientSocketHandler> { }
public class SetMovingTouchCommand : UnityEngine.Events.UnityEvent<UnityEngine.Vector2, UnityEngine.Vector2, string, AltClientSocketHandler> { }
public class DragObjectCommand : UnityEngine.Events.UnityEvent<UnityEngine.Vector2, AltUnityObject, AltClientSocketHandler> { }
public class DropObjectCommand : UnityEngine.Events.UnityEvent<UnityEngine.Vector2, AltUnityObject, AltClientSocketHandler> { }
public class PointerUpCommand : UnityEngine.Events.UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class PointerDownCommand : UnityEngine.Events.UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class TiltCommand : UnityEngine.Events.UnityEvent<UnityEngine.Vector3, AltClientSocketHandler> { }
public class TapCommand : UnityEngine.Events.UnityEvent<AltUnityObject, AltClientSocketHandler> { }

public class PointerEnterCommand : UnityEngine.Events.UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class PointerExitCommand : UnityEngine.Events.UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class LoadSceneCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }

public class DeleteKeyPlayerPrefCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }
public class DeletePlayerPrefCommand : UnityEngine.Events.UnityEvent< AltClientSocketHandler> { }
public class GetKeyPlayerPrefCommand : UnityEngine.Events.UnityEvent<string,PLayerPrefKeyType, AltClientSocketHandler> { }
public class SetKeyPlayerPrefCommand : UnityEngine.Events.UnityEvent<string,string,PLayerPrefKeyType, AltClientSocketHandler> { }

public class SwipeFinishedCommand: UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }


public class GetAllComponentsCommand : UnityEngine.Events.UnityEvent<string, AltClientSocketHandler> { }
public class GetAllFieldsCommand : UnityEngine.Events.UnityEvent<string,AltUnityComponent, AltClientSocketHandler> { }
public class GetAllMethodsCommand : UnityEngine.Events.UnityEvent<AltUnityComponent, AltClientSocketHandler> { }
public class GetAllScenesCommand: UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }
public class GetAllCamerasCommand: UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }

public class GetScreenshotCommand: UnityEngine.Events.UnityEvent<UnityEngine.Vector2, AltClientSocketHandler> { }
public class HighlightObjectScreenshotCommand : UnityEngine.Events.UnityEvent<int,string, UnityEngine.Vector2, AltClientSocketHandler> { }
public class HighlightObjectFromCoordinatesScreenshotCommand : UnityEngine.Events.UnityEvent<UnityEngine.Vector2, string, UnityEngine.Vector2, AltClientSocketHandler> { }
public class ScreenshotReady: UnityEngine.Events.UnityEvent <UnityEngine.Texture2D, UnityEngine.Vector2, AltClientSocketHandler> { }

public class SetTimeScaleCommand : UnityEngine.Events.UnityEvent<float, AltClientSocketHandler> { }
public class GetTimeScaleCommand : UnityEngine.Events.UnityEvent<AltClientSocketHandler> { }

public class AltUnityEvents
{

    public UnityEngine.Events.UnityEvent ResponseReceived;

    public FindObjectByNameCommand FindObjectByName;
    public FindObjectWhereNameContainsCommand FindObjectWhereNameContains;
    public FindObjectByComponentCommand FindObjectByComponent;

    public FindObjectsByNameCommand FindObjectsByName;
    public FindObjectsWhereNameContainsCommand FindObjectsWhereNameContains;
    public FindObjectsByComponentCommand FindObjectsByComponent;

    public GetCurrentSceneCommand GetCurrentScene;
    public GetAllObjectsCommand GetAllObjects;

    public ClickEventCommand ClickEvent;
    public TapScreenCommand TapScreen;
    public TapCommand Tap;
    public GetComponentPropertyCommand GetComponentProperty;
    public SetComponentPropertyCommand SetComponentProperty;
    public CallComponentMethodCommand CallComponentMethod;
    public GetTextCommand GetText;

    public SetStationaryTouchCommand SetStationaryTouch;
    public SetMovingTouchCommand SetMovingTouch;
    public DragObjectCommand DragObject;
    public DropObjectCommand DropObject;
    public PointerUpCommand PointerUp;
    public PointerDownCommand PointerDown;
    public PointerEnterCommand PointerEnter;
    public PointerExitCommand PointerExit;
    public TiltCommand Tilt;

    public CloseConnectionCommand CloseConnection;
    public UnknownStringCommand UnknownString;

    public LoadSceneCommand LoadScene;
    public SetKeyPlayerPrefCommand SetKeyPlayerPref;
    public DeleteKeyPlayerPrefCommand DeleteKeyPlayerPref;
    public DeletePlayerPrefCommand DeletePlayerPref;
    public GetKeyPlayerPrefCommand GetKeyPlayerPref;

    public SwipeFinishedCommand SwipeFinished;

    public GetAllComponentsCommand GetAllComponents;
    public GetAllFieldsCommand GetAllFields;
    public GetAllMethodsCommand GetAllMethods;
    public GetAllScenesCommand GetAllScenes;
    public GetAllCamerasCommand GetAllCameras;
    public GetScreenshotCommand GetScreenshot;
    public HighlightObjectScreenshotCommand HighlightObjectScreenshot;
    public HighlightObjectFromCoordinatesScreenshotCommand HighlightObjectFromCoordinates;

    public ScreenshotReady ScreenshotReady;
    public SetTimeScaleCommand SetTimeScale;
    public GetTimeScaleCommand GetTimeScale;

    // We are a singleton!
    private static AltUnityEvents _instance;
    public static AltUnityEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AltUnityEvents();

                _instance.FindObjectByName = new FindObjectByNameCommand();
                _instance.FindObjectWhereNameContains = new FindObjectWhereNameContainsCommand();
                _instance.FindObjectByComponent = new FindObjectByComponentCommand();

                _instance.FindObjectsByName = new FindObjectsByNameCommand();
                _instance.FindObjectsWhereNameContains = new FindObjectsWhereNameContainsCommand();
                _instance.FindObjectsByComponent = new FindObjectsByComponentCommand();

                _instance.GetAllObjects = new GetAllObjectsCommand();
                _instance.GetCurrentScene = new GetCurrentSceneCommand();


                _instance.ClickEvent = new ClickEventCommand();
                _instance.TapScreen = new TapScreenCommand();
                _instance.Tap = new TapCommand();
                _instance.GetComponentProperty = new GetComponentPropertyCommand();
                _instance.SetComponentProperty = new SetComponentPropertyCommand();
                _instance.CallComponentMethod = new CallComponentMethodCommand();
                _instance.GetText = new GetTextCommand();

                _instance.UnknownString = new UnknownStringCommand();
                _instance.ResponseReceived = new UnityEngine.Events.UnityEvent();
                _instance.CloseConnection = new CloseConnectionCommand();

                _instance.SetMovingTouch = new SetMovingTouchCommand();
                _instance.SetStationaryTouch = new SetStationaryTouchCommand();
                _instance.DragObject = new DragObjectCommand();
                _instance.DropObject = new DropObjectCommand();
                _instance.PointerUp = new PointerUpCommand();
                _instance.PointerDown = new PointerDownCommand();
                _instance.Tilt = new TiltCommand();
                _instance.PointerExit = new PointerExitCommand();
                _instance.PointerEnter = new PointerEnterCommand();

                _instance.LoadScene = new LoadSceneCommand();
                _instance.SetKeyPlayerPref=new SetKeyPlayerPrefCommand();
                _instance.GetKeyPlayerPref=new GetKeyPlayerPrefCommand();
                _instance.DeleteKeyPlayerPref=new DeleteKeyPlayerPrefCommand();
                _instance.DeletePlayerPref=new DeletePlayerPrefCommand();
                _instance.SwipeFinished=new SwipeFinishedCommand();


                _instance.GetAllComponents=new GetAllComponentsCommand();
                _instance.GetAllMethods=new GetAllMethodsCommand();
                _instance.GetAllFields=new GetAllFieldsCommand();
                _instance.GetAllScenes=new GetAllScenesCommand();
                _instance.GetAllCameras = new GetAllCamerasCommand();
                _instance.GetScreenshot=new GetScreenshotCommand();
                _instance.HighlightObjectFromCoordinates = new HighlightObjectFromCoordinatesScreenshotCommand();
                _instance.HighlightObjectScreenshot = new HighlightObjectScreenshotCommand();

                _instance.ScreenshotReady = new ScreenshotReady();
                _instance.SetTimeScale = new SetTimeScaleCommand();
                _instance.GetTimeScale = new GetTimeScaleCommand();
            }
            return _instance;
        }
    }
}