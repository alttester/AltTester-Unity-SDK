package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;

public class AltWaitForObjectToNotBePresent extends AltBaseFindObject {
    private AltWaitForObjectsParameters altWaitForObjectsParameters;
    public AltWaitForObjectToNotBePresent(AltBaseSettings altBaseSettings, AltWaitForObjectsParameters altWaitForObjectsParameters) {
        super(altBaseSettings);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
    }
    public void Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time <= altWaitForObjectsParameters.getTimeout()) {
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
