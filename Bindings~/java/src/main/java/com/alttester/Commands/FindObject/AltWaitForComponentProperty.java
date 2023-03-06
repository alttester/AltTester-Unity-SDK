package com.alttester.Commands.FindObject;

import com.alttester.Utils;
import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.altTesterExceptions.WaitTimeOutException;

/**
 * Wait until there are no longer any objects that respect the given criteria or
 * times run out and will throw an error.
 */
public class AltWaitForComponentProperty<T> extends AltBaseFindObject {
    /**
     * @param altFindObjectsParameters the properties parameter for finding the
     *                                 objects in a scene.
     */
    private AltObject obj;
    private AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams;

    /**
     * @param messageHandler
     * @param altWaitForComponentPropertyParams
     */
    public AltWaitForComponentProperty(IMessageHandler messageHandler,
            AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams) {
        super(messageHandler);
        this.altWaitForComponentPropertyParams = altWaitForComponentPropertyParams;
    }

    public T Execute(Class<T> returnType) {
        T propertyFound;

        double time = 0;
        while (time < altWaitForComponentPropertyParams.getTimeout()) {
            logger.debug("Waiting for element where name contains "
                    + altWaitForComponentPropertyParams.getAltGetComponentPropertyParams().getPropertyName() + "....");
            try {
                propertyFound = obj.getComponentProperty(
                        altWaitForComponentPropertyParams.getAltGetComponentPropertyParams(),
                        returnType);

                if (propertyFound.equals(
                        altWaitForComponentPropertyParams.getPropertyValue())) {
                    return propertyFound;
                }
            } catch (Exception e) {
                logger.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            Utils.sleepFor(altWaitForComponentPropertyParams.getInterval());
            time += altWaitForComponentPropertyParams.getInterval();
        }
        throw new WaitTimeOutException(

                "Property " + altWaitForComponentPropertyParams.getAltGetComponentPropertyParams().getPropertyName()
                        + " still not found after " + altWaitForComponentPropertyParams.getTimeout() + " seconds");
    }
}
