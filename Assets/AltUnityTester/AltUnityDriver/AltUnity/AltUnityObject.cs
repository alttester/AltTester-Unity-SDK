using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;


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
    [JsonIgnore]
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

    public Vector2 getScreenPosition()
    {
        return new Vector2(x, y);
    }

    public Vector3 getWorldPosition()
    {
        return new Vector3(worldX, worldY, worldZ);
    }
    public String GetComponentProperty(String componentName, String propertyName, String assemblyName = null)
    {
        String altObject = JsonConvert.SerializeObject(this);
        String propertyInfo = JsonConvert.SerializeObject(new AltUnityObjectProperty(componentName, propertyName,assemblyName));
        altUnityDriver.Socket.Client.Send(
            Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getObjectComponentProperty", altObject , propertyInfo )));
        String data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return data;
        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public String SetComponentProperty(String componentName, String propertyName, String value, String assemblyName = null)
    {
        String altObject = JsonConvert.SerializeObject(this);
        String propertyInfo = JsonConvert.SerializeObject(new AltUnityObjectProperty(componentName, propertyName,assemblyName));
        altUnityDriver.Socket.Client.Send(
            Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("setObjectComponentProperty",altObject , propertyInfo , value )));
        String data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return data;
        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public String CallComponentMethod(String componentName, String methodName,String parameters,String typeOfParameters="", String assemblyName = null)
    {
        String altObject = JsonConvert.SerializeObject(this);
        String actionInfo =
            JsonConvert.SerializeObject(new AltUnityObjectAction(componentName, methodName, parameters,typeOfParameters, assemblyName));
        altUnityDriver.Socket.Client.Send(
            Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("callComponentMethodForObject", altObject , actionInfo )));
        String data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return data;
        AltUnityDriver.HandleErrors(data);
        return null;


    }

    public String GetText()
    {
        return GetComponentProperty("UnityEngine.UI.Text", "text",null);
    }
    
    public AltUnityObject ClickEvent()
    {
        String altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("clickEvent" ,altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject DragObject(Vector2 position)
    {
        String positionString = JsonConvert.SerializeObject(position, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        // Debug.Log("position string:" + positionString);
        string altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("dragObject",positionString , altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public AltUnityObject DropObject(Vector2 position)
    {
        string altObject = JsonConvert.SerializeObject(this);
        String positionString = JsonConvert.SerializeObject(position, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("dropObject", positionString , altObject)));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
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
        string altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerUpFromObject", altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
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
        string altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerDownFromObject" , altObject)));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
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
        string altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerEnterObject", altObject)));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
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
        string altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("pointerExitObject",altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
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
        string altObject = JsonConvert.SerializeObject(this);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("tapObject",altObject )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }

        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public List<AltUnityComponent> GetAllComponents()
    {
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getAllComponents", id.ToString() )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityComponent>>(data);
        AltUnityDriver.HandleErrors(data);
        return null;
    }

    public List<AltUnityProperty> GetAllProperties(AltUnityComponent altUnityComponent)
    {
        var altComponent = JsonConvert.SerializeObject(altUnityComponent);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getAllFields",id.ToString() , altComponent)));
        string data = altUnityDriver.Recvall(); 
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<AltUnityProperty>>(data);
        AltUnityDriver.HandleErrors(data);
        return null;
    }
    public List<string> GetAllMethods(AltUnityComponent altUnityComponent)
    {
        var altComponent = JsonConvert.SerializeObject(altUnityComponent);
        altUnityDriver.Socket.Client.Send(Encoding.ASCII.GetBytes(altUnityDriver.CreateCommand("getAllMethods",altComponent )));
        string data = altUnityDriver.Recvall();
        if (!data.Contains("error:")) return JsonConvert.DeserializeObject<List<string>>(data);
        AltUnityDriver.HandleErrors(data);
        return null;
    }
}