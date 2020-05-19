package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

/**
 * Wait until it finds an object that respect the given criteria or times run out and will throw an error.
 */
public class AltWaitForObjectWhichContains extends AltBaseFindObject {

    /**
     * @param altWaitForObjectsParameters the properties parameter for finding the objects in a scene.
     */
    private AltWaitForObjectsParameters altWaitForObjectsParameters;
    public AltWaitForObjectWhichContains(AltBaseSettings altBaseSettings, AltWaitForObjectsParameters altWaitForObjectsParameters) {
        super(altBaseSettings);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForObjectsParameters.getTimeout()) {
            log.debug("Waiting for element where name contains " + altWaitForObjectsParameters.getAltFindObjectsParameters().getValue() + "....");
            try {
                altElement = new AltFindObjectWhichContains(altBaseSettings,altWaitForObjectsParameters.getAltFindObjectsParameters()).Execute();
                if (altElement != null) {
                    return altElement;
                }
            } catch (Exception e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            sleepFor(altWaitForObjectsParameters.getInterval());
            time += altWaitForObjectsParameters.getInterval();
        }
        throw new WaitTimeOutException("Element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getValue() + " still not found after " + altWaitForObjectsParameters.getTimeout()+ " seconds");
    }
}
