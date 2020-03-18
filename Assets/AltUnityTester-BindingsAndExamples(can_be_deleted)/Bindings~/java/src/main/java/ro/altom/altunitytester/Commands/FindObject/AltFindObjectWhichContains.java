package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Find the first object in the scene which respects the given criteria.
 */
public class AltFindObjectWhichContains extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;
    public AltFindObjectWhichContains(AltBaseSettings altBaseSettings, AltFindObjectsParameters altFindObjectsParameters) {
        super(altBaseSettings);
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
    public AltUnityObject Execute(){
        String path= SetPathContains(altFindObjectsParameters.getBy(),altFindObjectsParameters.getValue());
        send(CreateCommand("findObject", path, altFindObjectsParameters.getCameraName(), String.valueOf(altFindObjectsParameters.isEnabled())));
        return ReceiveAltUnityObject();
    }
}
