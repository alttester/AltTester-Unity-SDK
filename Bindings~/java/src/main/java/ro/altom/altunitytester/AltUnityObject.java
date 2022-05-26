package ro.altom.altunitytester;

import lombok.Getter;
import ro.altom.altunitytester.Commands.ObjectCommand.*;
import ro.altom.altunitytester.position.Vector2;
import ro.altom.altunitytester.position.Vector3;
import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.altunitytester.Commands.FindObject.AltFindObject;

@Getter
public class AltUnityObject {

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
    public int transformParentId;
    public int transformId;

    private transient IMessageHandler messageHandler;

    public IMessageHandler getMessageHandler() {
        return messageHandler;
    }

    public void setMesssageHandler(IMessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    public AltUnityObject() {

    }

    public AltUnityObject(String name, int id, int x, int y, int z, int mobileY, String type, boolean enabled,
            float worldX, float worldY, float worldZ, int idCamera, int transformParentId, int transformId) {
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
        this.transformId = transformId;
        this.transformParentId = transformParentId;
    }

    /**
     * Returns the parent of the AltUnity object on which it is called
     *
     * @return - The parent object
     */
    public AltUnityObject getParent() {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(By.PATH,
                "//*[@id=" + this.id + "]/..").build();
        AltUnityObject response = new AltFindObject(messageHandler, altFindObjectsParameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns the screen position of the AltUnity object
     *
     * @return - the screen position
     */
    public Vector2 getScreenPosition() {
        return new Vector2(this.x, this.y);
    }

    /**
     * Returns the world position of the AltUnity object
     *
     * @return - the world position
     */
    public Vector3 getWorldPosition() {
        return new Vector3(this.worldX, this.worldY, this.worldZ);
    }

    /**
     * Returns the value of the given component property.
     *
     * @return - the value of the given component property
     */
    public <T> T getComponentProperty(AltGetComponentPropertyParams altGetComponentPropertyParameters,
            Class<T> returnType) {
        altGetComponentPropertyParameters.setAltUnityObject(this);
        T response = new AltGetComponentProperty(messageHandler, altGetComponentPropertyParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets value of the given component property.
     */
    public void setComponentProperty(AltSetComponentPropertyParams altSetComponentPropertyParameters) {
        altSetComponentPropertyParameters.setAltUnityObject(this);
        new AltSetComponentProperty(messageHandler, altSetComponentPropertyParameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
    }

    /**
     * Invokes a method from an existing component of the object.
     */
    public <T> T callComponentMethod(AltCallComponentMethodParams altCallComponentMethodParameters,
            Class<T> returnType) {
        altCallComponentMethodParameters.setAltUnityObject(this);
        T response = new AltCallComponentMethod(messageHandler, altCallComponentMethodParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns text value from a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     */
    public String getText() {
        AltGetTextParams altGetTextParameters = new AltGetTextParams(this);
        String response = new AltGetText(messageHandler, altGetTextParameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets text value for a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     */
    public AltUnityObject setText(String text) {
        AltSetTextParams parameters = new AltSetTextParams.Builder(text).build();
        parameters.setAltUnityObject(this);

        AltUnityObject response = new AltSetText(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets text value for a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     */
    public AltUnityObject setText(AltSetTextParams parameters) {
        parameters.setAltUnityObject(this);
        AltUnityObject response = new AltSetText(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Simulates pointer up action on the object.
     */
    public AltUnityObject pointerUp() {
        return sendActionAndEvaluateResult("pointerUpFromObject");
    }

    /**
     * Simulates pointer down action on the object.
     */
    public AltUnityObject pointerDown() {
        return sendActionAndEvaluateResult("pointerDownFromObject");
    }

    /**
     * Simulates pointer enter action on the object.
     */
    public AltUnityObject pointerEnter() {
        return sendActionAndEvaluateResult("pointerEnterObject");
    }

    /**
     * Simulates pointer exit action on the object.
     */
    public AltUnityObject pointerExit() {
        return sendActionAndEvaluateResult("pointerExitObject");
    }

    /**
     * Tap current object.
     *
     * @return The clicked object
     */
    public AltUnityObject tap() {
        return tap(new AltTapClickElementParams.Builder().build());
    }

    /**
     * Tap current object
     *
     * @param parameters Tap parameters
     * @return The tapped object
     */
    public AltUnityObject tap(AltTapClickElementParams parameters) {
        parameters.setAltUnityObject(this);
        return new AltTapElement(messageHandler, parameters).Execute();
    }

    /**
     * Click current object.
     *
     * @return The clicked object
     */
    public AltUnityObject click() {
        AltTapClickElementParams params = new AltTapClickElementParams.Builder().build();
        return this.click(params);
    }

    /**
     * Click current object.
     *
     * @param parameters Click parameters
     * @return The clicked object
     */
    public AltUnityObject click(AltTapClickElementParams parameters) {
        parameters.setAltUnityObject(this);
        AltUnityObject response = new AltClickElement(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    private AltUnityObject sendActionAndEvaluateResult(String command) {
        AltUnityObject response = new AltSendActionAndEvaluateResult(messageHandler, this, command).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }
}
