using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FindObjectByNameCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectWhereNameContainsCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectByComponentCommand : UnityEvent<string, AltClientSocketHandler> { }

public class FindObjectsByNameCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectsWhereNameContainsCommand : UnityEvent<string, AltClientSocketHandler> { }
public class FindObjectsByComponentCommand : UnityEvent<string, AltClientSocketHandler> { }

public class GetCurrentSceneCommand : UnityEvent<AltClientSocketHandler> { }
public class GetAllObjectsCommand : UnityEvent<AltClientSocketHandler> { }
public class TapCommand : UnityEvent<string, AltClientSocketHandler> { }

public class GetComponentPropertyCommand : UnityEvent<string, string, AltClientSocketHandler> {} 
public class SetComponentPropertyCommand : UnityEvent<string, string, string, AltClientSocketHandler> {} 
public class CallComponentMethodCommand: UnityEvent<string, string, AltClientSocketHandler> {}
public class GetTextCommand: UnityEvent<string, AltClientSocketHandler>{}

public class CloseConnectionCommand : UnityEvent<AltClientSocketHandler> { }
public class UnknownStringCommand : UnityEvent<AltClientSocketHandler> { }

public class AltUnityEvents {

    public UnityEvent ResponseReceived;

    public FindObjectByNameCommand FindObjectByName;
    public FindObjectWhereNameContainsCommand FindObjectWhereNameContains;
    public FindObjectByComponentCommand FindObjectByComponent;

    public FindObjectsByNameCommand FindObjectsByName;
    public FindObjectsWhereNameContainsCommand FindObjectsWhereNameContains;    
    public FindObjectsByComponentCommand FindObjectsByComponent;

    public GetCurrentSceneCommand GetCurrentScene;
    public GetAllObjectsCommand GetAllObjects;
    
    public TapCommand Tap;
    public GetComponentPropertyCommand GetComponentProperty;
    public SetComponentPropertyCommand SetComponentProperty;
    public CallComponentMethodCommand CallComponentMethod;
    public GetTextCommand GetText;

    public CloseConnectionCommand CloseConnection;
    public UnknownStringCommand UnknownString;

    // We are a singleton!
    private static AltUnityEvents _instance;
    public static AltUnityEvents Instance {
        get {
            if (_instance == null) {
                _instance = new AltUnityEvents();

                _instance.FindObjectByName = new FindObjectByNameCommand();
                _instance.FindObjectWhereNameContains = new FindObjectWhereNameContainsCommand();
                _instance.FindObjectByComponent = new FindObjectByComponentCommand();

                _instance.FindObjectsByName = new FindObjectsByNameCommand();
                _instance.FindObjectsWhereNameContains = new FindObjectsWhereNameContainsCommand();                
                _instance.FindObjectsByComponent = new FindObjectsByComponentCommand();               
                
                _instance.GetAllObjects = new GetAllObjectsCommand();
                _instance.GetCurrentScene = new GetCurrentSceneCommand();

                _instance.Tap = new TapCommand();     
                _instance.GetComponentProperty = new GetComponentPropertyCommand();
                _instance.SetComponentProperty = new SetComponentPropertyCommand();
                _instance.CallComponentMethod = new CallComponentMethodCommand();
                _instance.GetText = new GetTextCommand();
                
                _instance.UnknownString = new UnknownStringCommand();
                _instance.ResponseReceived = new UnityEvent();
                _instance.CloseConnection = new CloseConnectionCommand();

            }
            return _instance;
        }
    }
}