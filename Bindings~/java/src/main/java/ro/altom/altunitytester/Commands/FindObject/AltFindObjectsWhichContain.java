package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Find all the objects in the scene which respect the given criteria.
 */
public class AltFindObjectsWhichContain extends AltBaseFindObject {
    /**
     * @param altFindObjectsParameters the properties parameter for finding the
     *                                 objects in a scene.
     */
    private AltFindObjectsParameters altFindObjectsParameters;

    public AltFindObjectsWhichContain(AltBaseSettings altBaseSettings,
            AltFindObjectsParameters altFindObjectsParameters) {
        super(altBaseSettings);
        this.altFindObjectsParameters = altFindObjectsParameters;
    }

    public AltUnityObject[] Execute() {
        String path = SetPathContains(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue());
        String cameraPath = SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraPath());
        send(CreateCommand("findObjects", path, altFindObjectsParameters.getCameraBy().toString(), cameraPath,
                String.valueOf(altFindObjectsParameters.isEnabled())));
        return ReceiveListOfAltUnityObjects();
    }
}
