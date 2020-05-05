package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.AltUnityObjectProperty;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSetComponentProperty extends AltBaseCommand {
    private AltUnityObject altUnityObject;
    private AltSetComponentPropertyParameters altSetComponentPropertyParameters;
    public AltSetComponentProperty(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject, AltSetComponentPropertyParameters altSetComponentPropertyParameters) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.altSetComponentPropertyParameters = altSetComponentPropertyParameters;
    }
    public String Execute(){
        String altObject = new Gson().toJson(altUnityObject);
        String propertyInfo = new Gson().toJson(new AltUnityObjectProperty(altSetComponentPropertyParameters.getAssembly(), altSetComponentPropertyParameters.getComponentName(), altSetComponentPropertyParameters.getPropertyName()));
        send(CreateCommand("setObjectComponentProperty",altObject,propertyInfo,altSetComponentPropertyParameters.getValue() ));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}
