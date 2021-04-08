package ro.altom.altunitytester.Commands;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObjectAction;

public class AltCallStaticMethod extends AltBaseCommand {
    AltCallStaticMethodParameters altCallStaticMethodParameters;

    public AltCallStaticMethod(AltBaseSettings altBaseSettings,
            AltCallStaticMethodParameters altCallStaticMethodParameters) {
        super(altBaseSettings);
        this.altCallStaticMethodParameters = altCallStaticMethodParameters;
    }

    public String Execute() {
        String actionInfo = new Gson().toJson(new AltUnityObjectAction(altCallStaticMethodParameters.getTypeName(),
                altCallStaticMethodParameters.getMethodName(), altCallStaticMethodParameters.getParameters(),
                altCallStaticMethodParameters.getTypeOfParameters(), altCallStaticMethodParameters.getAssembly()));
        SendCommand("callComponentMethodForObject", "", actionInfo);
        return recvall();
    }
}
