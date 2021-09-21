package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.IMessageHandler;
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

    public AltFindObjectsWhichContain(IMessageHandler messageHandler,
            AltFindObjectsParameters altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObjects");
    }

    public AltUnityObject[] Execute() {
        altFindObjectsParameters.setPath(SetPathContains(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveListOfAltUnityObjects(altFindObjectsParameters);
    }
}
