package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

public class AltFindObjectsWhichContains extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;
    public AltFindObjectsWhichContains(AltBaseSettings altBaseSettings, AltFindObjectsParameters altFindObjectsParameters) {
        super(altBaseSettings);
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
    public AltUnityObject[] Execute(){
        String path=SetPathContains(altFindObjectsParameters.getBy(),altFindObjectsParameters.getValue());
        send(CreateCommand("findObjects", path, altFindObjectsParameters.getCameraName(), String.valueOf(altFindObjectsParameters.isEnabled())));
        return ReceiveListOfAltUnityObjects();
    }
}
