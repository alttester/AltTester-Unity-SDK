
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
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        string propertyInfo = Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectProperty(componentName, propertyName,assemblyName));
        altUnityDriver.Socket.Client.Send(
             System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getObjectComponentProperty", altObject , propertyInfo )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return data;
        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public string SetComponentProperty(string componentName, string propertyName, string value, string assemblyName = null)
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        string propertyInfo = Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectProperty(componentName, propertyName,assemblyName));
        altUnityDriver.Socket.Client.Send(
            System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("setObjectComponentProperty",altObject , propertyInfo , value )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return data;
        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public string CallComponentMethod(string componentName, string methodName,string parameters,string typeOfParameters="", string assemblyName = null)
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        string actionInfo =
            Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectAction(componentName, methodName, parameters,typeOfParameters, assemblyName));
        altUnityDriver.Socket.Client.Send(
             System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("callComponentMethodForObject", altObject , actionInfo )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return data;
        AltUnityDriver.HandleErrors(data);
        return null;


    }

    public string GetText()
    {
        return GetComponentProperty("UnityEngine.UI.Text", "text",null);
    }
    
    public AltUnityObject ClickEvent()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("clickEvent" ,altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject DragObject(UnityEngine.Vector2 position)
    {
        string positionString = Newtonsoft.Json.JsonConvert.SerializeObject(position, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        // Debug.Log("position string:" + positionString);
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("dragObject",positionString , altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject DropObject(UnityEngine.Vector2 position)
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        string positionString = Newtonsoft.Json.JsonConvert.SerializeObject(position, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("dropObject", positionString , altObject)));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public AltUnityObject PointerUpFromObject()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerUpFromObject", altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject PointerDownFromObject()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerDownFromObject" , altObject)));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public AltUnityObject PointerEnterObject()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerEnterObject", altObject)));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject PointerExitObject()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerExitObject",altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject Tap()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("tapObject",altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public System.Collections.Generic.List<AltUnityComponent> GetAllComponents()
    {
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getAllComponents", id.ToString() )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityComponent>>(data);
        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public System.Collections.Generic.List<AltUnityProperty> GetAllProperties(AltUnityComponent altUnityComponent)
    {
        var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getAllFields",id.ToString() , altComponent)));
        string data = altUnityDriver.Recvall(); 
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityProperty>>(data);
        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public System.Collections.Generic.List<string> GetAllMethods(AltUnityComponent altUnityComponent)
    {
        var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
        altUnityDriver.Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getAllMethods",altComponent )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<string>>(data);
        AltUnityDriver.HandleErrors(data);
        return null;
    }
}