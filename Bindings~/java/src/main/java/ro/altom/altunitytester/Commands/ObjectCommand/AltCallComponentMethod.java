package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.AltUnityObjectAction;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Invoke a method from an existing component of the object.
 */
public class AltCallComponentMethod extends AltBaseCommand {
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;
    /**
     * @param altCallComponentMethodParameters builder for calling component methods
     */
    private AltCallComponentMethodParameters altCallComponentMethodParameters;

    public AltCallComponentMethod(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject,
            AltCallComponentMethodParameters altCallComponentMethodParameters) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.altCallComponentMethodParameters = altCallComponentMethodParameters;
    }

    public String Execute() {
        String altObject = new Gson().toJson(altUnityObject);
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(
                altCallComponentMethodParameters.getComponentName(), altCallComponentMethodParameters.getMethodName(),
                altCallComponentMethodParameters.getParameters(),
                altCallComponentMethodParameters.getTypeOfParameters(),
                altCallComponentMethodParameters.getAssembly()));
        SendCommand("callComponentMethodForObject", altObject, actionInfo);
        return recvall();
    }
}
