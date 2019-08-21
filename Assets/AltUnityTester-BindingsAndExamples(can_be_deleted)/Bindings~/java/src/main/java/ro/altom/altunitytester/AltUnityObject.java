package ro.altom.altunitytester;

import com.google.gson.Gson;
import lombok.Getter;

@Getter
public class AltUnityObject {
    // TODO: decouple AltUnityObject from the driver instance
    static AltUnityDriver altUnityDriver;
    // TODO: encapsulate state
    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getName()}
     */
    public String name;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getId()}
     */
    public int id;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getX()}
     */
    public int x;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getY()}
     */
    public int y;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getZ()}
     */
    public int z;
    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getMobileY()}
     */
    public int mobileY;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getType()}
     */
    public String type;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #isEnabled()}
     */
    public boolean enabled;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getWorldX()}
     */
    public float worldX;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getWorldY()}
     */
    public float worldY;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getWorldZ()}
     */
    public float worldZ;

    /**
     * Access to this variable will be removed in the future.
     *
     * As of version 1.3.0 use getter {@link #getIdCamera()}
     */
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

    public String getComponentProperty(String assemblyName, String componentName, String propertyName) {
        String altObject = new Gson().toJson(this);
        String propertyInfo = new Gson().toJson(new AltUnityObjectProperty(assemblyName, componentName, propertyName));
        altUnityDriver.send(altUnityDriver.CreateCommand("getObjectComponentProperty", altObject,propertyInfo ));
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) {
            return data;
        }
        altUnityDriver.handleErrors(data);
        return "";
    }

    public String getComponentProperty(String componentName, String propertyName) {
        return getComponentProperty("", componentName, propertyName);
    }

    public String setComponentProperty(String assemblyName, String componentName, String propertyName, String value) {
        String altObject = new Gson().toJson(this);
        String propertyInfo = new Gson().toJson(new AltUnityObjectProperty(assemblyName, componentName, propertyName));
        altUnityDriver.send(altUnityDriver.CreateCommand("setObjectComponentProperty",altObject,propertyInfo,value ));
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) {
            return data;
        }
        altUnityDriver.handleErrors(data);
        return "";
    }

    public String setComponentProperty(String componentName, String propertyName, String value) {
        return setComponentProperty("", componentName, propertyName, value);
    }

    public String callComponentMethod(String assemblyName, String componentName, String methodName, String parameters, String typeOfParameters) {
        String altObject = new Gson().toJson(this);
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(componentName,methodName,parameters,typeOfParameters,assemblyName));
        altUnityDriver.send(altUnityDriver.CreateCommand("callComponentMethodForObject",altObject ,actionInfo ));
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) return data;
        altUnityDriver.handleErrors(data);
        return null;
    }

    public String callComponentMethod(String componentName, String methodName, String parameters) throws Exception {
        return callComponentMethod(componentName, methodName, parameters, "", "");
    }

    public String getText() {
        return getComponentProperty("UnityEngine.UI.Text", "text");
    }

    public AltUnityObject clickEvent() {
        return sendActionAndEvaluateResult("clickEvent");
    }

    public AltUnityObject drag(int x, int y) {
        return sendActionWithCoordinateAndEvaluate(x, y, "dragObject");
    }

    public AltUnityObject drop(int x, int y) {
        return sendActionWithCoordinateAndEvaluate(x, y, "dropObject");
    }

    public AltUnityObject pointerUp() {
        return sendActionAndEvaluateResult("pointerUpFromObject");
    }

    public AltUnityObject pointerDown() {
        return sendActionAndEvaluateResult("pointerDownFromObject");
    }

    public AltUnityObject pointerEnter() {
        return sendActionAndEvaluateResult("pointerEnterObject");
    }

    public AltUnityObject pointerExit() {
        return sendActionAndEvaluateResult("pointerExitObject");
    }

    public AltUnityObject tap() {
        return sendActionAndEvaluateResult("tapObject");
    }

    private AltUnityObject sendActionAndEvaluateResult(String s) {
        String altObject = new Gson().toJson(this);
        altUnityDriver.send(altUnityDriver.CreateCommand(s, altObject ));
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        altUnityDriver.handleErrors(data);
        return null;
    }

    private AltUnityObject sendActionWithCoordinateAndEvaluate(int x, int y, String s) {
        String positionString = altUnityDriver.vectorToJsonString(x, y);
        String altObject = new Gson().toJson(this);
        altUnityDriver.send(altUnityDriver.CreateCommand(s ,positionString, altObject ));
        String data = altUnityDriver.recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        altUnityDriver.handleErrors(data);
        return null;
    }
}
