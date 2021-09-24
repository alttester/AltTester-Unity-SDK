package ro.altom.altunitytester;

import lombok.Getter;
import ro.altom.altunitytester.Commands.ObjectCommand.*;
import ro.altom.altunitytester.position.Vector2;
import ro.altom.altunitytester.position.Vector3;
import ro.altom.altunitytester.AltUnityDriver.By;
import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
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

    public AltUnityObject(){
        
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

    public AltUnityObject getParent() {
        AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(By.PATH,
                "//*[@id=" + this.id + "]/..").build();
        return new AltFindObject(messageHandler, altFindObjectsParameters).Execute();
    }

    public Vector2 getScreenPosition() {
        return new Vector2(this.x, this.y);
    }

    public Vector3 getWorldPosition() {
        return new Vector3(this.worldX, this.worldY, this.worldZ);
    }

    public String getComponentProperty(AltGetComponentPropertyParameters altGetComponentPropertyParameters) {
        altGetComponentPropertyParameters.setAltUnityObject(this);
        return new AltGetComponentProperty(messageHandler, altGetComponentPropertyParameters).Execute();
    }

    public String getComponentProperty(String assemblyName, String componentName, String propertyName) {
        AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder(
                componentName, propertyName).withAssembly(assemblyName).build();
        return getComponentProperty(altGetComponentPropertyParameters);
    }

    public String getComponentProperty(String componentName, String propertyName) {
        return getComponentProperty("", componentName, propertyName);
    }

    public String setComponentProperty(AltSetComponentPropertyParameters altSetComponentPropertyParameters) {
        altSetComponentPropertyParameters.setAltUnityObject(this);
        return new AltSetComponentProperty(messageHandler, altSetComponentPropertyParameters).Execute();
    }

    public String setComponentProperty(String assemblyName, String componentName, String propertyName, String value) {
        AltSetComponentPropertyParameters altSetComponentPropertyParameters = new AltSetComponentPropertyParameters.Builder(
                componentName, propertyName, value).withAssembly(assemblyName).build();
        return setComponentProperty(altSetComponentPropertyParameters);
    }

    public String setComponentProperty(String componentName, String propertyName, String value) {
        return setComponentProperty("", componentName, propertyName, value);
    }

    public <T> T callComponentMethod(AltCallComponentMethodParameters altCallComponentMethodParameters, Class<T> returnType) {
        altCallComponentMethodParameters.setAltUnityObject(this);
        return new AltCallComponentMethod(messageHandler, altCallComponentMethodParameters).Execute(returnType);
    }

    public <T> T callComponentMethod(String assemblyName, String componentName, String methodName, Object[] parameters,
            String[] typeOfParameters, Class<T> returnType) {
        AltCallComponentMethodParameters altCallComponentMethodParameters = new AltCallComponentMethodParameters.Builder(
                componentName, methodName, parameters).withTypeOfParameters(typeOfParameters).withAssembly(assemblyName)
                        .build();
        return callComponentMethod(altCallComponentMethodParameters, returnType);
    }

    public <T> T callComponentMethod(String componentName, String methodName, Object[] parameters, Class<T> returnType) throws Exception {
        return callComponentMethod("", componentName, methodName, parameters, null, returnType);
    }

    public String getText() {
        AltGetTextParameters altGetTextParameters = new AltGetTextParameters(this);
        return new AltGetText(messageHandler, altGetTextParameters).Execute();
    }

    public AltUnityObject setText(String text) {
        AltSetTextParameters altSetTextParameters = new AltSetTextParameters(text, this);
        return new AltSetText(messageHandler, altSetTextParameters).Execute();
    }

    @Deprecated()
    public AltUnityObject clickEvent() {
        return sendActionAndEvaluateResult("clickEvent");
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

    /**
     * Tap current object.
     * 
     * @return The clicked object
     */
    public AltUnityObject tap() {
        return tap(new AltTapClickElementParameters.Builder().build());
    }

    /**
     * Tap current object
     * 
     * @param parameters Tap parameters
     * @return The tapped object
     */
    public AltUnityObject tap(AltTapClickElementParameters parameters) {
        parameters.setAltUnityObject(this);
        return new AltTapElement(messageHandler, parameters).Execute();
    }

    /**
     * Click current object.
     * 
     * @param parameters Click parameters
     * @return The clicked object
     */
    public AltUnityObject click(AltTapClickElementParameters parameters) {
        parameters.setAltUnityObject(this);
        return new AltClickElement(messageHandler, parameters).Execute();
    }

    private AltUnityObject sendActionAndEvaluateResult(String command) {
        return new AltSendActionAndEvaluateResult(messageHandler, this, command).Execute();
    }
}
