package com.alttester.Commands.FindObject;

import com.alttester.AltDriver.By;
import com.alttester.AltObject;
import com.alttester.IMessageHandler;

/**
 * Returns information about every object loaded in the currently loaded scenes.
 */
public class AltGetAllElements extends AltBaseFindObject {
    /**
     * @param altGetAllElementsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    private AltGetAllElementsParams altGetAllElementsParameters;

    public AltGetAllElements(IMessageHandler messageHandler, AltGetAllElementsParams altGetAllElementsParameters) {
        super(messageHandler);
        this.altGetAllElementsParameters = altGetAllElementsParameters;
        this.altGetAllElementsParameters.setCommandName("findObjects");
    }

    public AltObject[] Execute() {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(By.PATH, "//*")
                .withCamera(altGetAllElementsParameters.getCameraBy(), altGetAllElementsParameters.getCameraValue())
                .build();
        return new AltFindObjects(messageHandler, altFindObjectsParameters).Execute();
    }
}
