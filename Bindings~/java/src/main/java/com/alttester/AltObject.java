/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester;

import lombok.Getter;

import com.alttester.Commands.ObjectCommand.*;
import com.alttester.Commands.UnityCommand.AltGetVisualElementProperty;
import com.alttester.Commands.UnityCommand.AltGetVisualElementProperyParams;
import com.alttester.altTesterExceptions.WrongAltObjectTypeException;
import com.alttester.position.Vector2;
import com.alttester.position.Vector3;
import com.alttester.AltDriver.By;
import com.alttester.Commands.FindObject.AltFindObjectsParams;
import com.alttester.Commands.FindObject.AltWaitForComponentProperty;
import com.alttester.Commands.FindObject.AltWaitForComponentPropertyParams;
import com.alttester.Commands.FindObject.AltFindObject;

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

    public AltObject UpdateObject() {
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
     * Returns the parent of the AltTester® object on which it is called
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
     * Returns the screen position of the AltTester® object
     *
     * @return - The screen position
     */
    public Vector2 getScreenPosition() {
        return new Vector2(this.x, this.y);
    }

    /**
     * Returns the world position of the AltTester® object
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
    public <T> T getComponentProperty(AltGetComponentPropertyParams altGetComponentPropertyParameters,
            Class<T> returnType) {
        altGetComponentPropertyParameters.setAltObject(this);
        T response = new AltGetComponentProperty(messageHandler, altGetComponentPropertyParameters).Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    /**
     * Wait until a property has a specific value and returns the value of the given
     * component property.
     *
     * 
     * @param altWaitForComponentPropertyParams -AltGetComponentPropertyParams
     *                                          altGetComponentPropertyParams* ,
     *                                          double timeout , double interval , T
     *                                          propertyValue* , AltObject obj*.
     * @param propertyValue                     - The value of the property expected
     * @param returnType                        - The type of the property
     * @return - The value of the given component property
     */
    @Deprecated
    public <T> T WaitForComponentProperty(AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams,
            T propertyValue,
            Class<T> returnType) {

        return waitForComponentProperty(altWaitForComponentPropertyParams, propertyValue, returnType);
    }

    public <T> T waitForComponentProperty(AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams,
            T propertyValue,
            Class<T> returnType) {

        altWaitForComponentPropertyParams.setAltObject(this);
        T response = new AltWaitForComponentProperty<T>(messageHandler,
                altWaitForComponentPropertyParams,
                propertyValue, this)
                .Execute(returnType);
        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return response;
    }

    public <T> T waitForComponentProperty(AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams,
            T propertyValue, Boolean getPropertyAsString,
            Class<T> returnType) {

        altWaitForComponentPropertyParams.setAltObject(this);
        T response = new AltWaitForComponentProperty<T>(messageHandler,
                altWaitForComponentPropertyParams,
                propertyValue, getPropertyAsString, this)
                .Execute(returnType);
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
    public void setComponentProperty(AltSetComponentPropertyParams altSetComponentPropertyParameters) {
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
     *
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

    /**
     * Retrieves the specified property value of a VisualElement object.
     *
     * @param propertyName The name of the property to retrieve.
     * @param returnType   The expected return type of the property value.
     * @param <T>          The type of the property value.
     * @return The value of the specified property.
     * @throws WrongAltObjectTypeException if the object type is not "UIToolkit".
     */
    public <T> T GetVisualElementProperty(String propertyName, Class<T> returnType) {
        if (!type.equals("UIToolkit")) {
            throw new WrongAltObjectTypeException("This method is only available for VisualElement objects");
        }
        AltGetVisualElementProperyParams altGetVisualElementPropertyParams = new AltGetVisualElementProperyParams.Builder(
                propertyName).build();
        altGetVisualElementPropertyParams.setAltObject(this);
        T propertyValue = new AltGetVisualElementProperty(messageHandler, altGetVisualElementPropertyParams)
                .Execute(returnType);

        Utils.sleepFor(messageHandler.getDelayAfterCommand());
        return propertyValue;
    }
}
