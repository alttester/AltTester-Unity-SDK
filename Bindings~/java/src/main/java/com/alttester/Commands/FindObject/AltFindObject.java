package com.alttester.Commands.FindObject;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;

/**
 * Builder for finding the first object in the scene that respects the given
 * criteria. It's no longer possible to search for object by name giving a path
 * in the hierarchy. For searching by name, use searching by path.
 */
public class AltFindObject extends AltBaseFindObject {
    private AltFindObjectsParams altFindObjectsParameters;

    /**
     * @param altFindObjectsParameters the properties parameter for finding the
     *                                 objects in a scene.
     * @param messageHandler
     */
    public AltFindObject(IMessageHandler messageHandler, AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObject");
    }

    public AltObject Execute() {
        altFindObjectsParameters
                .setPath(SetPath(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveAltObject(altFindObjectsParameters);
    }
}
