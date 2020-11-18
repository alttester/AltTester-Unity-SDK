package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Find the first object in the scene which respects the given criteria.
 */
public class AltFindObjectWhichContains extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;

    public AltFindObjectWhichContains(AltBaseSettings altBaseSettings,
            AltFindObjectsParameters altFindObjectsParameters) {
        super(altBaseSettings);
        this.altFindObjectsParameters = altFindObjectsParameters;
    }

    public AltUnityObject Execute() {
        String path = SetPathContains(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue());
        String cameraPath = SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraPath());
        SendCommand("findObject", path, altFindObjectsParameters.getCameraBy().toString(), cameraPath,
                String.valueOf(altFindObjectsParameters.isEnabled()));
        return ReceiveAltUnityObject();
    }
}
