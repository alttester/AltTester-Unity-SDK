using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class FindObjectByNameCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectWhereNameContainsCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectByComponentCommand : UnityEvent<string, AltClientSocketHandler> { }

public class FindObjectsByNameCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectsWhereNameContainsCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectsByComponentCommand : UnityEvent<string, AltClientSocketHandler> { }

public class GetCurrentSceneCommand : UnityEvent<AltClientSocketHandler> { }
public class GetAllObjectsCommand : UnityEvent<string, AltClientSocketHandler> { }

public class ClickEventCommand : UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class TapScreenCommand : UnityEvent<string, string, AltClientSocketHandler> { }

public class GetComponentPropertyCommand : UnityEvent<string, string, AltClientSocketHandler> { }
public class SetComponentPropertyCommand : UnityEvent<string, string, string, AltClientSocketHandler> { }
public class CallComponentMethodCommand : UnityEvent<string, string, AltClientSocketHandler> { }
public class GetTextCommand : UnityEvent<string, AltClientSocketHandler> { }

public class CloseConnectionCommand : UnityEvent<AltClientSocketHandler> { }
public class UnknownStringCommand : UnityEvent<AltClientSocketHandler> { }

public class SetStationaryTouchCommand : UnityEvent<Touch, string, AltClientSocketHandler> { }
public class SetMovingTouchCommand : UnityEvent<Vector2, Vector2, string, AltClientSocketHandler> { }
public class DragObjectCommand : UnityEvent<Vector2, AltUnityObject, AltClientSocketHandler> { }
public class DropObjectCommand : UnityEvent<Vector2, AltUnityObject, AltClientSocketHandler> { }
public class PointerUpCommand : UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class PointerDownCommand : UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class TiltCommand : UnityEvent<Vector3, AltClientSocketHandler> { }
public class TapCommand : UnityEvent<AltUnityObject, AltClientSocketHandler> { }

public class PointerEnterCommand : UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class PointerExitCommand : UnityEvent<AltUnityObject, AltClientSocketHandler> { }
public class LoadSceneCommand : UnityEvent<string, AltClientSocketHandler> { }

public class DeleteKeyPlayerPrefCommand : UnityEvent<string, AltClientSocketHandler> { }
public class DeletePlayerPrefCommand : UnityEvent< AltClientSocketHandler> { }
public class GetKeyPlayerPrefCommand : UnityEvent<string,PLayerPrefKeyType, AltClientSocketHandler> { }
public class SetKeyPlayerPrefCommand : UnityEvent<string,string,PLayerPrefKeyType, AltClientSocketHandler> { }

public class SwipeFinishedCommand: UnityEvent<AltClientSocketHandler> { }


public class GetAllComponentsCommand : UnityEvent<string, AltClientSocketHandler> { }
public class GetAllFieldsCommand : UnityEvent<string,AltUnityComponent, AltClientSocketHandler> { }
public class GetAllMethodsCommand : UnityEvent<AltUnityComponent, AltClientSocketHandler> { }
public class GetAllScenesCommand: UnityEvent<AltClientSocketHandler> { }
public class GetAllCamerasCommand: UnityEvent<AltClientSocketHandler> { }

public class GetScreenshotCommand: UnityEvent<Vector2,AltClientSocketHandler> { }
public class HighlightObjectScreenshotCommand : UnityEvent<int,string, Vector2, AltClientSocketHandler> { }
public class HighlightObjectFromCoordinatesScreenshotCommand : UnityEvent<Vector2,string, Vector2, AltClientSocketHandler> { }
public class ScreenshotReady: UnityEvent <Texture2D, Vector2, AltClientSocketHandler> { }

public class SetTimeScaleCommand : UnityEvent<float, AltClientSocketHandler> { }
public class GetTimeScaleCommand : UnityEvent<AltClientSocketHandler> { }

public class AltUnityEvents
{

    public UnityEvent ResponseReceived;

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
                _instance.ResponseReceived = new UnityEvent();
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