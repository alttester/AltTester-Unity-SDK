package com.alttester.Commands.FindObject;

import com.alttester.AltObject;
import com.alttester.IMessageHandler;
import com.alttester.Utils;
import com.alttester.altTesterExceptions.WaitTimeOutException;

/**
 * Wait until it finds an object that respect the given criteria or times run
 * out and will throw an error.
 */
public class AltWaitForObjectWhichContains extends AltBaseFindObject {

    /**
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    private AltWaitForObjectsParams altWaitForObjectsParameters;

    public AltWaitForObjectWhichContains(IMessageHandler messageHandler,
            AltWaitForObjectsParams altWaitForObjectsParameters) {
        super(messageHandler);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
        this.altWaitForObjectsParameters.setCommandName("findObject");
    }

    public AltObject Execute() throws WaitTimeOutException {
        double time = 0;
        AltObject altElement = null;
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
            Utils.sleepFor(altWaitForObjectsParameters.getInterval());
            time += altWaitForObjectsParameters.getInterval();
        }
        throw new WaitTimeOutException("Element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath()
                + " still not found after " + altWaitForObjectsParameters.getTimeout() + " seconds");
    }
}
