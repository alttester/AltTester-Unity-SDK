package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Find all the objects in the scene which respect the given criteria.
 */
public class AltFindObjectsWhichContains extends AltBaseFindObject {
    /**
     * @param altFindObjectsParameters the properties parameter for finding the objects in a scene.
     */
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
