package ro.altom.alttester.Commands.FindObject;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltObject;

/**
 * Find all the objects in the scene which respect the given criteria.
 */
public class AltFindObjectsWhichContain extends AltBaseFindObject {
    /**
     * @param altFindObjectsParameters the properties parameter for finding the
     *                                 objects in a scene.
     */
    private AltFindObjectsParams altFindObjectsParameters;

    public AltFindObjectsWhichContain(IMessageHandler messageHandler,
            AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObjects");
    }

    public AltObject[] Execute() {
        altFindObjectsParameters
                .setPath(SetPathContains(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveListOfAltObjects(altFindObjectsParameters);
    }
}
