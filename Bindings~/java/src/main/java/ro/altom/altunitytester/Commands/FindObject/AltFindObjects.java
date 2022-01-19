package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Builder for finding all objects in the scene that respect the given criteria.
 * It's no longer possible to search for object by name giving a path in the
 * hierarchy. For searching by name, use searching by path.
 */
public class AltFindObjects extends AltBaseFindObject {
    private AltFindObjectsParams altFindObjectsParameters;

    /**
     * @param altFindObjectsParameters the properties parameter for finding it in a
     *                                 scene.
     */
    public AltFindObjects(IMessageHandler messageHandler, AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObjects");
    }

    public AltUnityObject[] Execute() {
        altFindObjectsParameters
                .setPath(SetPath(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveListOfAltUnityObjects(altFindObjectsParameters);
    }
}
