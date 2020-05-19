package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Builder for finding all objects in the scene that respect the given criteria. 
 * It's no longer possible to search for object by name giving a path in the hierarchy. For searching by name, use searching by path.
 */
public class AltFindObjects extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;

    /**
     * @param altFindObjectsParameters the properties parameter for finding it in a scene.
     */
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
