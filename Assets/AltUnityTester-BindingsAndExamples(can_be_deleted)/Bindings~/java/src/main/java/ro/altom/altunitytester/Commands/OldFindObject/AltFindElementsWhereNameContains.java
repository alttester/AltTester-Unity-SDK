package ro.altom.altunitytester.Commands.OldFindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltFindElementsWhereNameContains extends AltCommandReturningAltObjects {
    private AltFindElementsParameters altFindElementsParameters;
    public AltFindElementsWhereNameContains(AltBaseSettings altBaseSettings, AltFindElementsParameters altFindElementsParameters) {
        super(altBaseSettings);
        this.altFindElementsParameters = altFindElementsParameters;
    }
    public AltUnityObject[] Execute(){
        send(CreateCommand("findObjectsWhereNameContains", altFindElementsParameters.getName(), altFindElementsParameters.getCameraName(), String.valueOf(altFindElementsParameters.isEnabled())));
        return ReceiveListOfAltUnityObjects();
    }
}
