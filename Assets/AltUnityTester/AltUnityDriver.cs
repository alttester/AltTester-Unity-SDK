using UnityEngine;
using System;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Reflection;
using Newtonsoft.Json;

public class AltUnityDriver : MonoBehaviour, AltIClientSocketHandlerDelegate {

    private Vector3 position = new Vector3();
    private AltSocketServer socketServer;

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

    private JsonSerializerSettings jsonSettings;
                


    public int socketPortNumber = 13000;
    public bool debugBuildNeeded = true;

    private static AltResponseQueue responseQueue;

    void Start() {
        jsonSettings = new JsonSerializerSettings();
        jsonSettings.NullValueHandling = NullValueHandling.Ignore;

        responseQueue = new AltResponseQueue();

        AltUnityEvents.Instance.FindObjectByName.AddListener(FindObjectByName);
        AltUnityEvents.Instance.FindObjectWhereNameContains.AddListener(FindObjectWhereNameContains);
        AltUnityEvents.Instance.FindObjectByComponent.AddListener(FindObjectByComponent);

        AltUnityEvents.Instance.FindObjectsByName.AddListener(FindObjectsByName);
        AltUnityEvents.Instance.FindObjectsWhereNameContains.AddListener(FindObjectsWhereNameContains);
        AltUnityEvents.Instance.FindObjectsByComponent.AddListener(FindObjectsByComponent);

        AltUnityEvents.Instance.GetAllObjects.AddListener(GetAllObjects);
        AltUnityEvents.Instance.GetCurrentScene.AddListener(GetCurrentScene);

        AltUnityEvents.Instance.Tap.AddListener(TapObjectByName);
        AltUnityEvents.Instance.GetComponentProperty.AddListener(GetObjectComponentProperty);
        AltUnityEvents.Instance.SetComponentProperty.AddListener(SetObjectComponentProperty);
        AltUnityEvents.Instance.CallComponentMethod.AddListener(CallComponentMethodForObject);
        AltUnityEvents.Instance.GetText.AddListener(GetText);

        AltUnityEvents.Instance.CloseConnection.AddListener(CloseConnection);
        AltUnityEvents.Instance.UnknownString.AddListener(UnknownString);

        if (debugBuildNeeded && !Debug.isDebugBuild) {
            Debug.Log("AltUnityTester will not run if this is not a Debug/Development build");
        } else {
            DontDestroyOnLoad(this);
            StartSocketServer();
            Debug.Log("AltUnity Driver started");
        }
    }

    public void StartSocketServer() {
        AltIClientSocketHandlerDelegate clientSocketHandlerDelegate = this;
        int maxClients = 1;
        string separatorString = "&";
        Encoding encoding = Encoding.UTF8;

        this.socketServer = new AltSocketServer(
            clientSocketHandlerDelegate, socketPortNumber, maxClients, separatorString, encoding);

        this.socketServer.StartListeningForConnections();

        Debug.Log(String.Format(
            "AltUnity Server at {0} on port {1}",
            this.socketServer.LocalEndPoint.Address, this.socketServer.PortNumber));
    }

    void OnApplicationQuit() {
        CleanUp();
    }

    public void CleanUp() {
        Debug.Log("Cleaning up socket server");
        this.socketServer.Cleanup();
    }

    private AltUnityObject GameObjectToAltUnityObject(GameObject gameObject) {
        if (Camera.current) {
            position = Camera.current.WorldToScreenPoint(gameObject.transform.position);
            if (gameObject.GetComponent<Collider>() != null) {
                position = Camera.current.WorldToScreenPoint(gameObject.GetComponent<Collider>().bounds.center);
            }
            if (gameObject.GetComponent<RectTransform>() != null) {
                position = gameObject.GetComponent<RectTransform>().position;
            }
        }
        string data;
        try {
            data = gameObject.transform.GetComponentInChildren<Text>().text;
        } catch (NullReferenceException) {
            data = "";
        }
        string parentName = "";
        if (gameObject.transform.parent != null) {
            parentName = gameObject.transform.parent.name;
        }
        AltUnityObject altObject = new AltUnityObject(name: gameObject.name,
                                                      id: gameObject.GetInstanceID(),
                                                      x: Convert.ToInt32(Mathf.Round(position.x)),
                                                      y: Convert.ToInt32(Mathf.Round(position.y)),
                                                      mobileY: Convert.ToInt32(Mathf.Round(Screen.height - position.y)),
                                                      text: data,
                                                      type: "",
                                                      enabled: true);
        return altObject;
    }

    public void ClientSocketHandlerDidReadMessage(AltClientSocketHandler handler, string message) {
        string[] pieces = message.Split(';');

        switch (pieces[0]) {
            case "findAllObjects":
                Debug.Log("all objects requested");
                AltUnityEvents.Instance.GetAllObjects.Invoke(handler);
                break;
            case "findObjectByName":
                Debug.Log("find object by name " + pieces[1]);
                AltUnityEvents.Instance.FindObjectByName.Invoke(pieces[1], handler);
                break;
            case "findObjectWhereNameContains":
                Debug.Log("find object where name contains:" + pieces[1]);
                AltUnityEvents.Instance.FindObjectWhereNameContains.Invoke(pieces[1], handler);
                break;
            case "tapObjectByName":
                Debug.Log("tap object by name " + pieces[1]);
                AltUnityEvents.Instance.Tap.Invoke(pieces[1], handler);
                break;
            case "findObjectsByName":
                Debug.Log("find multiple objects by name " + pieces[1]);
                AltUnityEvents.Instance.FindObjectsByName.Invoke(pieces[1], handler);
                break;
            case "findObjectsWhereNameContains":
                Debug.Log("find objects where name contains:" + pieces[1]);
                AltUnityEvents.Instance.FindObjectsWhereNameContains.Invoke(pieces[1], handler);
                break;
            case "getCurrentScene":
                Debug.Log("get current scene");
                AltUnityEvents.Instance.GetCurrentScene.Invoke(handler);
                break;
            case "findObjectByComponent":
                Debug.Log("find object by component " + pieces[1]);
                AltUnityEvents.Instance.FindObjectByComponent.Invoke(pieces[1], handler);
                break;
            case "findObjectsByComponent":
                Debug.Log("find objects by component " + pieces[1]);
                AltUnityEvents.Instance.FindObjectsByComponent.Invoke(pieces[1], handler);
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
            case "getText":
                Debug.Log("get text for object " + pieces[1]);
                AltUnityEvents.Instance.GetText.Invoke(pieces[1], handler);
                break;
            case "closeConnection":
                Debug.Log("Socket connection closed!");
                AltUnityEvents.Instance.CloseConnection.Invoke(handler);
                break;
            default:
                AltUnityEvents.Instance.UnknownString.Invoke(handler);
                break;
        }
    }

    private void FindObjectByName(string name, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            GameObject gameObject = GameObject.Find(name);
            string response = errorNotFoundMessage;
            if (gameObject != null)
                response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject));
            handler.SendResponse(response);
        });
    }

    private void FindObjectWhereNameContains(string name, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = errorNotFoundMessage;
            foreach (GameObject testableObject in FindObjectsOfType<GameObject>()) {
                if (testableObject.name.Contains(name)) {
                    response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(testableObject));
                    break;
                }
            }
            handler.SendResponse(response);
        });
    }

    private void FindObjectByComponent(string componentTypeName, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = errorNotFoundMessage;
            Type componentType = GetType(componentTypeName);
            if (componentType != null) {
                GameObject gameObject = null;
                foreach (GameObject testableObject in FindObjectsOfType<GameObject>()) {
                    if (testableObject.GetComponent(componentType) != null) {
                        gameObject = testableObject;
                        response = JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject));
                        break;
                    }
                }
            } else {
                response = errorComponentNotFoundMessage;
            }
            handler.SendResponse(response);
        });
    }

    private void FindObjectsByName(string name, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            List<AltUnityObject> foundObjects = new List<AltUnityObject>();
            foreach (GameObject testableObject in FindObjectsOfType<GameObject>()) {
                if (name == "" || testableObject.name == name) {
                    foundObjects.Add(GameObjectToAltUnityObject(testableObject));
                }
            }
            handler.SendResponse(JsonConvert.SerializeObject(foundObjects));
        });
    }

    private void FindObjectsByComponent(string componentTypeName, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = "";
            List<AltUnityObject> foundObjects = new List<AltUnityObject>();
            Type componentType = GetType(componentTypeName);
            if (componentType != null) {
                foreach (GameObject testableObject in FindObjectsOfType<GameObject>()) {
                    if (name == "" || testableObject.GetComponent(componentType) != null) {
                        foundObjects.Add(GameObjectToAltUnityObject(testableObject));
                    }
                }
                response = JsonConvert.SerializeObject(foundObjects);
            } else {
                response = errorComponentNotFoundMessage;
            }
            handler.SendResponse(response);
        });
    }

    private void FindObjectsWhereNameContains(string name, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            List<AltUnityObject> foundObjects = new List<AltUnityObject>();
            foreach (GameObject testableObject in FindObjectsOfType<GameObject>()) {
                if (testableObject.name.Contains(name)) {
                    foundObjects.Add(GameObjectToAltUnityObject(testableObject));
                }
            }
            handler.SendResponse(JsonConvert.SerializeObject(foundObjects));
        });
    }

    private void GetAllObjects(AltClientSocketHandler handler) {
        FindObjectsByName("", handler);
    }

    private void GetCurrentScene(AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            AltUnityObject scene = new AltUnityObject(name: SceneManager.GetActiveScene().name,
                                                       type: "UnityScene");
            handler.SendResponse(JsonUtility.ToJson(scene));
        });
    }

    private void TapObjectByName(string name, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            GameObject gameObject = GameObject.Find(name);
            EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
            if (trigger != null) {
                gameObject.GetComponent<EventTrigger>().SendMessage("OnMouseDown");
            } else {
                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.submitHandler);
            }
            handler.SendResponse(JsonConvert.SerializeObject(GameObjectToAltUnityObject(gameObject)));
        });
    }

    private void GetObjectComponentProperty(string altObjectString, string propertyString, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = errorPropertyNotFoundMessage;
            try {
                AltUnityObjectProperty altProperty = JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
                AltUnityObject altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                GameObject testableObject = GetGameObject(altUnityObject);
                MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
                response = GetValueForMember(memberInfo, testableObject, altProperty);
            } catch (JsonException e) {
                Debug.Log(e);
                if (altObjectString.Contains("error")) {
                    response = errorObjectWasNotFound;
                } else {
                    response = errorCouldNotParseJsonString;
                }
            } catch (NullReferenceException) {
                response = errorComponentNotFoundMessage;
            }
            handler.SendResponse(response);
        });
    }

    private void SetObjectComponentProperty(string altObjectString, string propertyString, string valueString, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = errorPropertyNotFoundMessage;
            try {
                AltUnityObjectProperty altProperty = JsonConvert.DeserializeObject<AltUnityObjectProperty>(propertyString);
                AltUnityObject altUnityObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                GameObject testableObject = GetGameObject(altUnityObject);
                MemberInfo memberInfo = GetMemberForObjectComponent(altUnityObject, altProperty);
                response = SetValueForMember(memberInfo, valueString, testableObject, altProperty);
            } catch (JsonException e) {
                Debug.Log(e);
                if (altObjectString.Contains("error")) {
                    response = errorObjectWasNotFound;
                } else {
                    response = errorCouldNotParseJsonString;
                }
            } catch (NullReferenceException e) {
                Debug.Log(e);
                response = errorComponentNotFoundMessage;
            }
            handler.SendResponse(response);
        });
    }

    private void CallComponentMethodForObject(string altObjectString, string actionString, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = errorMethodNotFoundMessage;
            Type componentType = null;
            try {
                AltUnityObject altObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                AltUnityObjectAction altAction = JsonConvert.DeserializeObject<AltUnityObjectAction>(actionString);
                componentType = GetType(altAction.component);
                MethodInfo methodInfo = componentType.GetMethod(altAction.method);
                GameObject gameObject = GetGameObject(altObject);

                if (gameObject.GetComponent(componentType) != null) {
                    if (methodInfo != null) {
                        if (altAction.parameters == "") {
                            methodInfo.Invoke(gameObject.GetComponent(componentType), null);
                        } else {
                            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                            string[] parameterStrings = altAction.parameters.Split('?');
                            if (parameterInfos.Length != parameterStrings.Length) 
                                throw new TargetParameterCountException();
                            object[] parameters = new object[parameterInfos.Length];
                            for (int i = 0; i < parameterInfos.Length; i++) {
                                if (parameterInfos[i].ParameterType == typeof(System.String))
                                    parameters[i] = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(parameterStrings[i]), parameterInfos[i].ParameterType);
                                else {
                                    parameters[i] = JsonConvert.DeserializeObject(parameterStrings[i], parameterInfos[i].ParameterType);
                                }
                            }
                            methodInfo.Invoke(gameObject.GetComponent(componentType), parameters);
                        }
                        response = "methodInvoked";
                    }
                }
            } catch (ArgumentException) {
                response = errorFailedToParseArguments;
            } catch (TargetParameterCountException) {
                response = errorIncorrectNumberOfParameters;
            } catch (JsonException e) {
                Debug.Log(e);
                if (altObjectString.Contains("error")) {
                    response = errorObjectWasNotFound;
                } else {
                    response = errorCouldNotParseJsonString;
                }
            } catch (NullReferenceException) {
                response = errorComponentNotFoundMessage;
            }
            handler.SendResponse(response);
        });
    }

    private void GetText(string altObjectString, AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            string response = "";
            try {
                AltUnityObject altObject = JsonConvert.DeserializeObject<AltUnityObject>(altObjectString);
                response = altObject.text;
            } catch (JsonException e) {
                Debug.Log(e);
                if (altObjectString.Contains("error")) {
                    response = errorObjectWasNotFound;
                } else {
                    response = errorCouldNotParseJsonString;
                }
            } catch (NullReferenceException) {
                response = errorNotFoundMessage;
            }
            handler.SendResponse(response);
        });
    }

    private void CloseConnection(AltClientSocketHandler handler) {
        socketServer.StartListeningForConnections();
    }

    private void UnknownString(AltClientSocketHandler handler) {
        responseQueue.ScheduleResponse(delegate {
            handler.SendResponse(errorCouldNotPerformOperationMessage);
        });
    }

    void Update() {
        responseQueue.Cycle();
    }

    public static Type GetType(string TypeName) {
        var type = Type.GetType(TypeName);

        if (type != null)
            return type;

        if (TypeName.Contains(".")) {
            var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));
            try {
                var assembly = Assembly.Load(assemblyName);
                if (assembly.GetType(TypeName) == null)
                    return null;
                return assembly.GetType(TypeName);
            } catch (Exception e) {
                return null;
            }
        }
        return null;
    }

    private static GameObject GetGameObject(AltUnityObject altUnityObject) {
        foreach (GameObject gameObject in FindObjectsOfType<GameObject>()) {
            if (gameObject.GetInstanceID() == altUnityObject.id)
                return gameObject;
        }
        return null;
    }

    private MemberInfo GetMemberForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectProperty altUnityObjectProperty) {
        MemberInfo memberInfo = null;
        Type componentType = null;
        componentType = GetType(altUnityObjectProperty.component);
        PropertyInfo propertyInfo = componentType.GetProperty(altUnityObjectProperty.property);
        FieldInfo fieldInfo = componentType.GetField(altUnityObjectProperty.property);
        if (GetGameObject(altUnityObject).GetComponent(componentType) != null) {
            if (propertyInfo != null)
                return propertyInfo;
            if (fieldInfo != null)
                return fieldInfo;
        }
        return memberInfo;
    }
    

    private MethodInfo GetMethodForObjectComponent(AltUnityObject altUnityObject, AltUnityObjectAction altUnityObjectAction) {
        Type componentType = null;
        componentType = GetType(altUnityObjectAction.component);
        MethodInfo methodInfo = componentType.GetMethod(altUnityObjectAction.method);
        return methodInfo;
    }

    private string GetValueForMember(MemberInfo memberInfo, GameObject testableObject, AltUnityObjectProperty altProperty) {
        string response = errorPropertyNotFoundMessage;
        if (memberInfo != null) {
            if (memberInfo.MemberType == MemberTypes.Property) {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                object value = propertyInfo.GetValue(testableObject.GetComponent(GetType(altProperty.component)), null);
                response = SerializeMemberValue(value, propertyInfo.PropertyType);
            }
            if (memberInfo.MemberType == MemberTypes.Field) {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                object value = fieldInfo.GetValue(testableObject.GetComponent(GetType(altProperty.component)));
                response = SerializeMemberValue(value, fieldInfo.FieldType);
            }
        }
        return response;
    }

    private string SetValueForMember(MemberInfo memberInfo, string valueString, GameObject testableObject, AltUnityObjectProperty altProperty) {
        string response = errorPropertyNotFoundMessage;
        if (memberInfo != null) {
            if (memberInfo.MemberType == MemberTypes.Property) {
                PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
                try {
                    object value = DeserializeMemberValue(valueString, propertyInfo.PropertyType);
                    if (value != null) {
                        propertyInfo.SetValue(testableObject.GetComponent(altProperty.component), value, null);
                        response = "valueSet";
                    } else
                        response = errorPropertyNotSet;
                } catch (Exception e) {
                    Debug.Log(e);
                    response = errorPropertyNotSet;
                }
            }
            if (memberInfo.MemberType == MemberTypes.Field) {
                FieldInfo fieldInfo = (FieldInfo)memberInfo;
                try {
                    object value = DeserializeMemberValue(valueString, fieldInfo.FieldType);
                    if (value != null) {
                        fieldInfo.SetValue(testableObject.GetComponent(altProperty.component), value);
                        response = "valueSet";
                    } else
                        response = errorPropertyNotSet;
                } catch (Exception e) {
                    Debug.Log(e);
                    response = errorPropertyNotSet;
                }
            }
        }
        return response;
    }

    private string SerializeMemberValue(object value, Type type) {
        string response = "";
        if (type == typeof(System.String))
            return value.ToString();
        try {
            response = JsonConvert.SerializeObject(value, type, jsonSettings);
        } catch (JsonException) {
            response = value.ToString();
        }
        return response;
    }

    private object DeserializeMemberValue(string valueString, Type type) {
        object value = null;
        if (type == typeof(System.String))
        valueString = JsonConvert.SerializeObject(valueString);
        try {
            value = JsonConvert.DeserializeObject(valueString, type);
        } catch (JsonException) {
            value = null; 
        }
        return value;
    }

}
