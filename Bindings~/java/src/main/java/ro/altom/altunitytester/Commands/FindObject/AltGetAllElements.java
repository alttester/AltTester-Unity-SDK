package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.AltUnityDriver.By;

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

    public AltUnityObject[] Execute() {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(By.PATH, "//*")
                .withCamera(altGetAllElementsParameters.getCameraBy(), altGetAllElementsParameters.getCameraValue())
                .build();
        return new AltFindObjects(messageHandler, altFindObjectsParameters).Execute();
    }
}
