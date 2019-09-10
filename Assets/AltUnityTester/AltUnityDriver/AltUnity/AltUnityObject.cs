
public class AltUnityObject
{
    public string name;
    public int id;
    public int x;
    public int y;
    public int z;
    public int mobileY;
    public string type;
    public bool enabled;
    public float worldX;
    public float worldY;
    public float worldZ;
    public int idCamera;
    public int parentId;
    public int transformId;
    [Newtonsoft.Json.JsonIgnore]
    public static AltUnityDriver altUnityDriver;
    public AltUnityObject(string name, int id = 0, int x = 0, int y = 0, int z = 0, int mobileY = 0, string type = "", bool enabled = true, float worldX = 0, float worldY = 0, float worldZ = 0, int idCamera = 0, int parentId = 0, int transformId = 0)
    {
        this.name = name;
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.mobileY = mobileY;
        this.type = type;
        this.enabled = enabled;
        this.worldX = worldX;
        this.worldY = worldY;
        this.worldZ = worldZ;
        this.idCamera = idCamera;
        this.parentId = parentId;
        this.transformId = transformId;
    }
    public UnityEngine.Vector2 getScreenPosition()
    {
        return new UnityEngine.Vector2(x, y);
    }
    public UnityEngine.Vector3 getWorldPosition()
    {
        return new UnityEngine.Vector3(worldX, worldY, worldZ);
    }
    public string GetComponentProperty(string componentName, string propertyName, string assemblyName = null)
    {
        return new GetComponentPropertyDriver(AltUnityDriver.socketSettings,componentName,propertyName,assemblyName,this).Execute();
    }
    public string SetComponentProperty(string componentName, string propertyName, string value, string assemblyName = null)
    {
        return new SetComponentPropertyDriver(AltUnityDriver.socketSettings,componentName,propertyName,value,assemblyName,this).Execute();
    }
    public string CallComponentMethod(string componentName, string methodName,string parameters,string typeOfParameters="", string assemblyName = null)
    {
        
        return new CallComponentMethodDriver(AltUnityDriver.socketSettings,componentName,methodName,parameters,typeOfParameters,assemblyName,this).Execute();
    }
    public string GetText()
    {
        return GetComponentProperty("UnityEngine.UI.Text", "text",null);
    }    
    public AltUnityObject ClickEvent()
    {
        return new ClickEventDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public AltUnityObject DragObject(UnityEngine.Vector2 position)
    {
        return new DragObjectDriver(AltUnityDriver.socketSettings,position,this).Execute();
    }
    public AltUnityObject DropObject(UnityEngine.Vector2 position)
    {
        return new DropObjectDriver(AltUnityDriver.socketSettings,position,this).Execute();   
    }
    public AltUnityObject PointerUpFromObject()
    {
        return new PointerUpFromObjectDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public AltUnityObject PointerDownFromObject()
    {
        return new PointerDownFromObjectDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public AltUnityObject PointerEnterObject()
    {
        return new PointerEnterObjectDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public AltUnityObject PointerExitObject()
    {
        return new PointerExitObjectDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public AltUnityObject Tap()
    {
        return new TapDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public System.Collections.Generic.List<AltUnityComponent> GetAllComponents()
    {
        return new GetAllComponentsDriver(AltUnityDriver.socketSettings,this).Execute();
    }
    public System.Collections.Generic.List<AltUnityProperty> GetAllProperties(AltUnityComponent altUnityComponent)
    {
       return new GetAllPropertiesDriver(AltUnityDriver.socketSettings,altUnityComponent,this).Execute();
    }
    public System.Collections.Generic.List<string> GetAllMethods(AltUnityComponent altUnityComponent)
    {
        return new GetAllMethodsDriver(AltUnityDriver.socketSettings,altUnityComponent,this).Execute();
    }
}