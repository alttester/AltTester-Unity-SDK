package ro.altom.altunitytester.Commands.OldFindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;

public class AltWaitForElementToNotBePresent extends AltBaseCommand {
    private AltWaitForElementParameters altWaitForElementParameters;
    public AltWaitForElementToNotBePresent(AltBaseSettings altBaseSettings, AltWaitForElementParameters altWaitForElementParameters) {
        super(altBaseSettings);
        this.altWaitForElementParameters = altWaitForElementParameters;
    }
    public void Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time <= altWaitForElementParameters.getTimeout()) {
            log.debug("Waiting for element " + altWaitForElementParameters.getAltFindObjectsParameters().getName() + " not to be present");
            try {
                altElement = new AltFindElement(altBaseSettings,altWaitForElementParameters.getAltFindObjectsParameters()).Execute();
                if (altElement == null) {
                    return;
                }
            } catch (Exception e) {
                log.warn(e.getLocalizedMessage());
                break;
            }
            sleepFor(altWaitForElementParameters.getInterval());
            time += altWaitForElementParameters.getInterval();
        }

        if (altElement != null) {
            throw new AltUnityException("Element " + altWaitForElementParameters.getAltFindObjectsParameters().getName() + " still found after " + altWaitForElementParameters.getTimeout()+ " seconds");
        }
    }
}
