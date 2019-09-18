package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.AltUnityObjectAction;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.Commands.AltCallStaticMethodsParameters;

public class AltCallComponentMethod extends AltBaseCommand {
    private AltUnityObject altUnityObject;
    private AltCallComponentMethodParameters altCallComponentMethodParameters;
    public AltCallComponentMethod(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject, AltCallComponentMethodParameters altCallComponentMethodParameters) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.altCallComponentMethodParameters = altCallComponentMethodParameters;
    }
    public String Execute(){
        String altObject = new Gson().toJson(altUnityObject);
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(altCallComponentMethodParameters.getAssembly(),altCallComponentMethodParameters.getMethodName(),altCallComponentMethodParameters.getParameters(),altCallComponentMethodParameters.getTypeOfParameters(),altCallComponentMethodParameters.getAssembly()));
        send(CreateCommand("callComponentMethodForObject",altObject ,actionInfo ));
        String data = recvall();
        if (!data.contains("error:")) return data;
        handleErrors(data);
        return null;
    }
}
