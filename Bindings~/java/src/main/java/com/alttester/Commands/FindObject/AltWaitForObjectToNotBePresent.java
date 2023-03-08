package com.alttester.Commands.FindObject;

import com.alttester.AltObject;
import com.alttester.IMessageHandler;
import com.alttester.Utils;
import com.alttester.Exceptions.AltException;

/**
 * Wait until the object in the scene that respect the given criteria is no
 * longer in the scene or times run out and will throw an error.
 */
public class AltWaitForObjectToNotBePresent extends AltBaseFindObject {

    /**
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    private AltWaitForObjectsParams altWaitForObjectsParameters;

    public AltWaitForObjectToNotBePresent(IMessageHandler messageHandler,
            AltWaitForObjectsParams altWaitForObjectsParameters) {
        super(messageHandler);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
    }

    public void Execute() {
        double time = 0;
        AltObject altElement = null;
        while (time <= altWaitForObjectsParameters.getTimeout()) {
            altElement = null;
            logger.debug("Waiting for element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath()
                    + " not to be present");
            try {
                altElement = new AltFindObject(messageHandler,
                        altWaitForObjectsParameters.getAltFindObjectsParameters()).Execute();
                if (altElement == null) {
                    return;
                }
            } catch (Exception e) {
                logger.warn(e.getLocalizedMessage());
                break;
            }
            Utils.sleepFor(altWaitForObjectsParameters.getInterval());
            time += altWaitForObjectsParameters.getInterval();
        }

        if (altElement != null) {
            throw new AltException(
                    "Element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath()
                            + " still found after " + altWaitForObjectsParameters.getTimeout() + " seconds");
        }
    }
}
