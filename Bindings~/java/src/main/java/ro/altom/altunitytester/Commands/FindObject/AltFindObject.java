package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;

/**
 * Builder for finding the first object in the scene that respects the given
 * criteria. It's no longer possible to search for object by name giving a path
 * in the hierarchy. For searching by name, use searching by path.
 */
public class AltFindObject extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;

    /**
     * @param altFindObjectsParameters the properties parameter for finding the
     *                                 objects in a scene.
     */
    public AltFindObject(IMessageHandler messageHandler, AltFindObjectsParameters altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObject");
    }

    public AltUnityObject Execute() {
        altFindObjectsParameters.setPath(SetPath(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveAltUnityObject(altFindObjectsParameters);
    }
}
