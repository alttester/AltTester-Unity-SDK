package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.AltUnityObjectProperty;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Get the value of a property from one of the component of the object.
 */
public class AltGetComponentProperty extends AltBaseCommand {
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;
    /**
     * @param altGetComponentPropertyParameters builder for getting components' property
     */
    private AltGetComponentPropertyParameters altGetComponentPropertyParameters;
    public AltGetComponentProperty(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject, AltGetComponentPropertyParameters altGetComponentPropertyParameters) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.altGetComponentPropertyParameters = altGetComponentPropertyParameters;
    }
    public String Execute(){
        String altObject = new Gson().toJson(altUnityObject);
        String propertyInfo = new Gson().toJson(new AltUnityObjectProperty(altGetComponentPropertyParameters.getAssembly(), altGetComponentPropertyParameters.getComponentName(), altGetComponentPropertyParameters.getPropertyName()));
        send(CreateCommand("getObjectComponentProperty", altObject,propertyInfo ));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}
