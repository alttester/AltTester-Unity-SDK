package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

/**
 * Wait until it finds an object that respect the given criteria or times run
 * out and will throw an error.
 */
public class AltWaitForObjectWhichContains extends AltBaseFindObject {

    /**
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    private AltWaitForObjectsParameters altWaitForObjectsParameters;

    public AltWaitForObjectWhichContains(IMessageHandler messageHandler,
            AltWaitForObjectsParameters altWaitForObjectsParameters) {
        super(messageHandler);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
        this.altWaitForObjectsParameters.setCommandName("findObject");
    }

    public AltUnityObject Execute() {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForObjectsParameters.getTimeout()) {
            logger.debug("Waiting for element where name contains "
                    + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath() + "....");
            try {
                altElement = new AltFindObjectWhichContains(messageHandler,
                        altWaitForObjectsParameters.getAltFindObjectsParameters()).Execute();
                if (altElement != null) {
                    return altElement;
                }
            } catch (Exception e) {
                logger.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            sleepFor(altWaitForObjectsParameters.getInterval());
            time += altWaitForObjectsParameters.getInterval();
        }
        throw new WaitTimeOutException("Element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath()
                + " still not found after " + altWaitForObjectsParameters.getTimeout() + " seconds");
    }
}
