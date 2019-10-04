package ro.altom.altunitytester.Commands;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObjectAction;

public class AltCallStaticMethods extends AltBaseCommand{
    AltCallStaticMethodsParameters altCallStaticMethodsParameters;
    public AltCallStaticMethods(AltBaseSettings altBaseSettings,AltCallStaticMethodsParameters altCallStaticMethodsParameters) {
        super(altBaseSettings);
        this.altCallStaticMethodsParameters=altCallStaticMethodsParameters;
    }
    public String Execute(){
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(altCallStaticMethodsParameters.getTypeName(), altCallStaticMethodsParameters.getMethodName(), altCallStaticMethodsParameters.getParameters(), altCallStaticMethodsParameters.getTypeOfParameters(), altCallStaticMethodsParameters.getAssembly()));
        send(CreateCommand("callComponentMethodForObject", "" , actionInfo ));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}
