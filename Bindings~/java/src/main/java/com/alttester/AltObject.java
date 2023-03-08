package com.alttester;

import com.alttester.AltDriver.By;
import com.alttester.Commands.FindObject.AltFindObject;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.ObjectCommand.AltCallComponentMethod;
import com.alttester.Commands.ObjectCommand.AltCallComponentMethodParams;
import com.alttester.Commands.ObjectCommand.AltClickElement;
import com.alttester.Commands.ObjectCommand.AltGetComponentProperty;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.Commands.ObjectCommand.AltGetText;
import com.alttester.Commands.ObjectCommand.AltGetTextParams;
import com.alttester.Commands.ObjectCommand.AltSendActionAndEvaluateResult;
import com.alttester.Commands.ObjectCommand.AltSetComponentProperty;
import com.alttester.Commands.ObjectCommand.AltSetComponentPropertyParams;
import com.alttester.Commands.ObjectCommand.AltSetText;
import com.alttester.Commands.ObjectCommand.AltSetTextParams;
import com.alttester.Commands.ObjectCommand.AltTapClickElementParams;
import com.alttester.Commands.ObjectCommand.AltTapElement;
import com.alttester.Position.Vector2;
import com.alttester.Position.Vector3;

import lombok.Getter;

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

    public final IMessageHandler getMessageHandler() {
        return messageHandler;
    }

    public final void setMesssageHandler(final IMessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    public AltObject() {

    }

    public AltObject(final String name, final int id, final int x, final int y, final int z, final int mobileY,
            final String type, final boolean enabled,
            final float worldX, final float worldY, final float worldZ, final int idCamera, final int transformParentId,
            final int transformId) {
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

    public final AltObject updateObject() {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(By.ID, String.valueOf(this.id))
                .build();
        AltObject altObject = new AltFindObject(messageHandler, altFindObjectsParameters).Execute();
        this.x = altObject.x;
        this.y = altObject.y;
        this.z = altObject.z;
        this.id = altObject.id;
        this.name = altObject.name;
        this.mobileY = altObject.mobileY;
        this.type = altObject.type;
        this.enabled = altObject.enabled;
        this.worldX = altObject.worldX;
        this.worldY = altObject.worldY;
        this.worldZ = altObject.worldZ;
        this.idCamera = altObject.idCamera;
        this.transformParentId = altObject.transformParentId;
        this.transformId = altObject.transformId;

        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return this;
    }

    /**
     * Returns the parent of the AltTester object on which it is called.
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
     * Returns the screen position of the AltTester object.
     *
     * @return - The screen position
     */
    public Vector2 getScreenPosition() {
        return new Vector2(this.x, this.y);
    }

    /**
     * Returns the world position of the AltTester object.
     *
     * @return - The world position
     */
    public Vector3 getWorldPosition() {
        return new Vector3(this.worldX, this.worldY, this.worldZ);
    }

    /**
     * Returns the value of the given component property.
     *
     * @param altGetComponentPropertyParameters - String componentName* , String
     *                                          propertyName* , String assembly ,
     *                                          int maxDepth
     * @return - The value of the given component property
     */
    public <T> T getComponentProperty(final AltGetComponentPropertyParams altGetComponentPropertyParameters,
            final Class<T> returnType) {
        altGetComponentPropertyParameters.setAltObject(this);
        T response = new AltGetComponentProperty(messageHandler, altGetComponentPropertyParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets value of the given component property.
     *
     * @param altSetComponentPropertyParameters - String componentName* , String
     *                                          propertyName* , String assembly ,
     *                                          String value*
     */
    public void setComponentProperty(final AltSetComponentPropertyParams altSetComponentPropertyParameters) {
        altSetComponentPropertyParameters.setAltObject(this);
        new AltSetComponentProperty(messageHandler, altSetComponentPropertyParameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
    }

    /**
     * Invokes a method from an existing component of the object.
     *
     * @param altCallComponentMethodParameters - String componentName* , String
     *                                         methodName* , Object[] parameters*,
     *                                         String[] typeOfParameters , String
     *                                         assembly
     * @param returnType
     * @return Actions of the method invoked
     */
    public <T> T callComponentMethod(final AltCallComponentMethodParams altCallComponentMethodParameters,
            final Class<T> returnType) {
        altCallComponentMethodParameters.setAltObject(this);
        T response = new AltCallComponentMethod(messageHandler, altCallComponentMethodParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Returns text value from a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     *
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
     *
     * @param text
     * @return Element that have text value changed
     */
    public AltObject setText(final String text) {
        AltSetTextParams parameters = new AltSetTextParams.Builder(text).build();
        parameters.setAltObject(this);

        AltObject response = new AltSetText(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Sets text value for a Button, Text, InputField. This also works with
     * TextMeshPro elements.
     *
     * @param parameters
     * @return Element that have text value changed
     */
    public AltObject setText(final AltSetTextParams parameters) {
        parameters.setAltObject(this);
        AltObject response = new AltSetText(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Simulates pointer up action on the object.
     *
     * @return Element that simulates pointer up action
     */
    public AltObject pointerUp() {
        return sendActionAndEvaluateResult("pointerUpFromObject");
    }

    /**
     * Simulates pointer down action on the object.
     *
     * @return Element that simulates pointer down action
     */
    public AltObject pointerDown() {
        return sendActionAndEvaluateResult("pointerDownFromObject");
    }

    /**
     * Simulates pointer enter action on the object.
     *
     * @return Element that simulates pointer enter action
     */
    public AltObject pointerEnter() {
        return sendActionAndEvaluateResult("pointerEnterObject");
    }

    /**
     * Simulates pointer exit action on the object.
     *
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
     * Tap current object.
     *
     * @param parameters Tap parameters
     * @return The tapped object
     */
    public AltObject tap(final AltTapClickElementParams parameters) {
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
    public AltObject click(final AltTapClickElementParams parameters) {
        parameters.setAltObject(this);
        AltObject response = new AltClickElement(messageHandler, parameters).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    private AltObject sendActionAndEvaluateResult(final String command) {
        AltObject response = new AltSendActionAndEvaluateResult(messageHandler, this, command).Execute();
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }
}
