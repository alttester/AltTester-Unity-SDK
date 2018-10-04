package ro.altom.altunitytester;

import com.google.gson.Gson;

public class AltUnityObject {
    public static AltUnityDriver altUnityDriver;

    public String name;
    public int id;
    public int x;
    public int y;
    public int z;
    public int mobileY;
    public String type;
    public boolean enabled;
    public float worldX;
    public float worldY;
    public float worldZ;
    public int idCamera;

    public AltUnityObject(String name, int id, int x, int y, int z, int mobileY, String type, boolean enabled, float worldX, float worldY, float worldZ, int idCamera) {
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
    }

    public String getComponentProperty(String assemblyName,String componentName, String propertyName) throws Exception {
        String altObject = new Gson().toJson(this);
        String propertyInfo = new Gson().toJson(new AltUnityObjectProperty(assemblyName,componentName, propertyName));
        altUnityDriver.send("getObjectComponentProperty;" + altObject + ";" + propertyInfo + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) return data;
        altUnityDriver.handleErrors(data);
        return null;
    }
    public String getComponentProperty(String componentName, String propertyName) throws Exception {
        return getComponentProperty("",componentName,propertyName);
    }


    public String setComponentProperty(String assemblyName,String componentName, String propertyName, String value) throws Exception {
        String altObject = new Gson().toJson(this);
        String propertyInfo = new Gson().toJson(new AltUnityObjectProperty(assemblyName,componentName, propertyName));
        altUnityDriver.send("setObjectComponentProperty;" + altObject + ";" + propertyInfo + ";" + value + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) return data;
        altUnityDriver.handleErrors(data);
        return null;
    }
    public String setComponentProperty(String componentName, String propertyName, String value) throws Exception {
        return setComponentProperty("",componentName,propertyName,value);
    }


    public String callComponentMethod(String assemblyName,String componentName, String methodName, String parameters,String typeOfParameters) throws Exception {
        String altObject = new Gson().toJson(this);
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(assemblyName,componentName, methodName, parameters,typeOfParameters));
        altUnityDriver.send("callComponentMethodForObject;" + altObject + ";" + actionInfo + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) return data;
        altUnityDriver.handleErrors(data);
        return null;
    }
    public String callComponentMethod(String componentName, String methodName, String parameters) throws Exception {
        return callComponentMethod(componentName,methodName,parameters,"","");
    }

    public String getText() throws Exception {
        return getComponentProperty("UnityEngine.UI.Text", "text");
    }

    public AltUnityObject clickEvent() throws Exception {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("clickEvent;" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        altUnityDriver.handleErrors(data);
        return null;
    }


    public AltUnityObject drag(int x, int y) throws Exception {
        String positionString = altUnityDriver.vectorToJsonString(x, y);
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("dragObject;" + positionString + ";" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
          return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }


    public AltUnityObject drop(int x, int y) throws Exception {
        String positionString = altUnityDriver.vectorToJsonString(x, y);
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("dropObject;" + positionString + ";" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
            return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }


    public AltUnityObject pointerUp() throws Exception {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("pointerUpFromObject;" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
            return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }

    public AltUnityObject pointerDown() throws Exception {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("pointerDownFromObject;" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
            return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }

    public AltUnityObject pointerEnter() throws Exception {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("pointerEnterObject;" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
            return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }

    public AltUnityObject pointerExit() throws Exception {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("pointerExitObject;" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
            return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }

    public AltUnityObject tap() throws Exception {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send("tapObject;" + altObject + ";&");
        String data = altUnityDriver.recvall();
        if (!data.contains("error:"))
        {
            return new Gson().fromJson(data, AltUnityObject.class);

        }

        altUnityDriver.handleErrors(data);
        return null;
    }
}
