package com.alttester.Commands.FindObject;

import com.alttester.AltObject;
import com.alttester.IMessageHandler;

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
     * @param messageHandler
     */
    public AltFindObjects(IMessageHandler messageHandler, AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObjects");
    }

    public AltObject[] Execute() {
        altFindObjectsParameters
                .setPath(SetPath(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveListOfAltObjects(altFindObjectsParameters);
    }
}
