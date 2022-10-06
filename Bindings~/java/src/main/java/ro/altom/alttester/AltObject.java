package ro.altom.alttester;

import lombok.Getter;
import ro.altom.alttester.Commands.ObjectCommand.*;
import ro.altom.alttester.position.Vector2;
import ro.altom.alttester.position.Vector3;
import ro.altom.alttester.AltDriver.By;
import ro.altom.alttester.Commands.FindObject.AltFindObjectsParams;
import ro.altom.alttester.Commands.FindObject.AltFindObject;

@Getter
public class AltObject {

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

    public AltObject() {

    }

    public AltObject(String name, int id, int x, int y, int z, int mobileY, String type, boolean enabled,
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
     * Returns the parent of the Alt object on which it is called
     *
     * @return - The parent object
     */
    public AltObject getParent() {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(By.PATH,
                "//*[@id=" + this.id + "]/..").build();
        AltObject response = new AltFindObject(messageHandler, altFindObjectsParameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns the screen position of the Alt object
     *
     * @return - The screen position
     */
    public Vector2 getScreenPosition() {
        return new Vector2(this.x, this.y);
    }

    /**
     * Returns the world position of the Alt object
     *
     * @return - The world position
     */
    public Vector3 getWorldPosition() {
        return new Vector3(this.worldX, this.worldY, this.worldZ);
    }

    /**
     * Returns the value of the given component property.
     * @param altGetComponentPropertyParameters - String componentName* , String propertyName* , String assembly , int maxDepth
     * @return - The value of the given component property
     */
    public <T> T getComponentProperty(AltGetComponentPropertyParams altGetComponentPropertyParameters,
            Class<T> returnType) {
        altGetComponentPropertyParameters.setAltObject(this);
        T response = new AltGetComponentProperty(messageHandler, altGetComponentPropertyParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets value of the given component property.
     * @param altSetComponentPropertyParameters - String componentName* , String propertyName* , String assembly , String value*
     */
    public void setComponentProperty(AltSetComponentPropertyParams altSetComponentPropertyParameters) {
        altSetComponentPropertyParameters.setAltObject(this);
        new AltSetComponentProperty(messageHandler, altSetComponentPropertyParameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
    }

    /**
     * Invokes a method from an existing component of the object.
     * @param altCallComponentMethodParameters - String componentName* , String methodName* , Object[] parameters*, String[] typeOfParameters , String assembly
     * @param returnType
     * @return Actions of the method invoked
     */
    public <T> T callComponentMethod(AltCallComponentMethodParams altCallComponentMethodParameters,
            Class<T> returnType) {
        altCallComponentMethodParameters.setAltObject(this);
        T response = new AltCallComponentMethod(messageHandler, altCallComponentMethodParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns text value from a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     * @return Text value 
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
     * @param text
     * @return Element that have text value changed
     */
    public AltObject setText(String text) {
        AltSetTextParams parameters = new AltSetTextParams.Builder(text).build();
        parameters.setAltObject(this);

        AltObject response = new AltSetText(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets text value for a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     * @param parameters
     * @return Element that have text value changed
     */
    public AltObject setText(AltSetTextParams parameters) {
        parameters.setAltObject(this);
        AltObject response = new AltSetText(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Simulates pointer up action on the object.
     * @return Element that simulates pointer up action 
     */
    public AltObject pointerUp() {
        return sendActionAndEvaluateResult("pointerUpFromObject");
    }

    /**
     * Simulates pointer down action on the object.
     * @return  Element that simulates pointer down action
     */
    public AltObject pointerDown() {
        return sendActionAndEvaluateResult("pointerDownFromObject");
    }

    /**
     * Simulates pointer enter action on the object.
     * @return Element that simulates pointer enter action
     */
    public AltObject pointerEnter() {
        return sendActionAndEvaluateResult("pointerEnterObject");
    }

    /**
     * Simulates pointer exit action on the object.
     * @return Element that simulates pointer exit action
     */
    public AltObject pointerExit() {
        return sendActionAndEvaluateResult("pointerExitObject");
    }

    /**
     * Tap current object.
     *
     * @return The clicked object
     */
    public AltObject tap() {
        return tap(new AltTapClickElementParams.Builder().build());
    }

    /**
     * Tap current object
     *
     * @param parameters Tap parameters
     * @return The tapped object
     */
    public AltObject tap(AltTapClickElementParams parameters) {
        parameters.setAltObject(this);
        return new AltTapElement(messageHandler, parameters).Execute();
    }

    /**
     * Click current object.
     *
     * @return The clicked object
     */
    public AltObject click() {
        AltTapClickElementParams params = new AltTapClickElementParams.Builder().build();
        return this.click(params);
    }

    /**
     * Click current object.
     *
     * @param parameters Click parameters
     * @return The clicked object
     */
    public AltObject click(AltTapClickElementParams parameters) {
        parameters.setAltObject(this);
        AltObject response = new AltClickElement(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    private AltObject sendActionAndEvaluateResult(String command) {
        AltObject response = new AltSendActionAndEvaluateResult(messageHandler, this, command).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }
}
