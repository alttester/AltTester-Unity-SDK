package ro.altom.alttester.Commands.FindObject;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltObject;

/**
 * Find the first object in the scene which respects the given criteria.
 */
public class AltFindObjectWhichContains extends AltBaseFindObject {
    private AltFindObjectsParams altFindObjectsParameters;

    public AltFindObjectWhichContains(IMessageHandler messageHandler,
            AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObject");
    }

    public AltObject Execute() {
        altFindObjectsParameters
                .setPath(SetPathContains(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveAltObject(altFindObjectsParameters);
    }
}
