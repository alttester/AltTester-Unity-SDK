package ro.altom.altunitytester.Commands.OldFindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForElementWhereNameContains extends AltBaseCommand {
    private AltWaitForElementParameters altWaitForElementParameters;
    public AltWaitForElementWhereNameContains(AltBaseSettings altBaseSettings, AltWaitForElementParameters altWaitForElementParameters) {
        super(altBaseSettings);
        this.altWaitForElementParameters = altWaitForElementParameters;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForElementParameters.getTimeout()) {
            log.debug("Waiting for element where name contains " + altWaitForElementParameters.getAltFindObjectsParameters().getName() + "....");
            try {
                altElement = new AltFindElementWhereNameContains(altBaseSettings,altWaitForElementParameters.getAltFindObjectsParameters()).Execute();
                if (altElement != null) {
                    return altElement;
                }
            } catch (Exception e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            sleepFor(altWaitForElementParameters.getInterval());
            time += altWaitForElementParameters.getInterval();
        }
        throw new WaitTimeOutException("Element " + altWaitForElementParameters.getAltFindObjectsParameters().getName() + " still not found after " + altWaitForElementParameters.getTimeout()+ " seconds");
    }
}
