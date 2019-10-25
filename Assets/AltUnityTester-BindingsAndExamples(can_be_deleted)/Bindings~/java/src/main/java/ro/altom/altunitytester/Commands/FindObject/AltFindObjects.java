package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

public class AltFindObjects extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;
    public AltFindObjects(AltBaseSettings altBaseSettings, AltFindObjectsParameters altFindObjectsParameters) {
        super(altBaseSettings);
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
    public AltUnityObject[] Execute(){
        String path=SetPath(altFindObjectsParameters.getBy(),altFindObjectsParameters.getValue());
        send(CreateCommand("findObjects", path, altFindObjectsParameters.getCameraName(), String.valueOf(altFindObjectsParameters.isEnabled())));
        return ReceiveListOfAltUnityObjects();
    }
}
