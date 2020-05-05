package ro.altom.altunitytester.Commands.OldFindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForElement extends AltBaseCommand {
    private AltWaitForElementParameters altWaitForElementParameters;
    public AltWaitForElement(AltBaseSettings altBaseSettings, AltWaitForElementParameters altWaitForElementParameters) {
        super(altBaseSettings);
        this.altWaitForElementParameters = altWaitForElementParameters;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForElementParameters.getTimeout()) {
            log.debug("Waiting for element " + altWaitForElementParameters.getAltFindObjectsParameters().getName() + "...");
            try {
                altElement = new AltFindElement(altBaseSettings,altWaitForElementParameters.getAltFindObjectsParameters()).Execute();
            } catch (Exception e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }

            if (altElement != null) {
                return altElement;
            }
            sleepFor(altWaitForElementParameters.getInterval());
            time += altWaitForElementParameters.getInterval();
        }

        throw new WaitTimeOutException("Element " + altWaitForElementParameters.getAltFindObjectsParameters().getName() + " not loaded after " + altWaitForElementParameters.getTimeout()+ " seconds");
    }
}
