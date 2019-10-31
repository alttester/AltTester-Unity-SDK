package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForObject extends AltBaseFindObject{
    private AltWaitForObjectsParameters altWaitForObjectsParameters;
    public AltWaitForObject(AltBaseSettings altBaseSettings, AltWaitForObjectsParameters altWaitForObjectsParameters) {
        super(altBaseSettings);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForObjectsParameters.getTimeout()) {
            log.debug("Waiting for element where name contains " + altWaitForObjectsParameters.getAltFindObjectsParameters() + "....");
            try {
                altElement = new AltFindObject(altBaseSettings,altWaitForObjectsParameters.getAltFindObjectsParameters()).Execute();
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
