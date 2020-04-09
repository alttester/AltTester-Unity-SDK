package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;

/**
 * Wait until the object in the scene that respect the given criteria is no longer in the scene or times run out and will throw an error.
 */
public class AltWaitForObjectToNotBePresent extends AltBaseFindObject {

    /**
     * @param altWaitForObjectsParameters the properties parameter for finding the objects in a scene.
     */
    private AltWaitForObjectsParameters altWaitForObjectsParameters;
    public AltWaitForObjectToNotBePresent(AltBaseSettings altBaseSettings, AltWaitForObjectsParameters altWaitForObjectsParameters) {
        super(altBaseSettings);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
    }
    public void Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time <= altWaitForObjectsParameters.getTimeout()) {
            altElement=null;
            log.debug("Waiting for element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getValue() + " not to be present");
            try {
                altElement = new AltFindObject(altBaseSettings,altWaitForObjectsParameters.getAltFindObjectsParameters()).Execute();
                if (altElement == null) {
                    return;
                }
            } catch (Exception e) {
                log.warn(e.getLocalizedMessage());
                break;
            }
            sleepFor(altWaitForObjectsParameters.getInterval());
            time += altWaitForObjectsParameters.getInterval();
        }

        if (altElement != null) {
            throw new AltUnityException("Element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getValue() + " still found after " + altWaitForObjectsParameters.getTimeout()+ " seconds");
        }
    }
}
