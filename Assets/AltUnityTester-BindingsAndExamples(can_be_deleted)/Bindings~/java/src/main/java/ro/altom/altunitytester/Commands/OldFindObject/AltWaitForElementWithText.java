package ro.altom.altunitytester.Commands.OldFindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForElementWithText extends AltBaseCommand {
    private AltWaitForElementWithTextParameters altWaitForElementWithTextParameters;
    public AltWaitForElementWithText(AltBaseSettings altBaseSettings, AltWaitForElementWithTextParameters altWaitForElementWithTextParameters) {
        super(altBaseSettings);
        this.altWaitForElementWithTextParameters = altWaitForElementWithTextParameters;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForElementWithTextParameters.getTimeout()) {
            log.debug("Waiting for element " +altWaitForElementWithTextParameters.getAltFindElementsParameters().getName()+ " to have text [" + altWaitForElementWithTextParameters.getText() + "]");
            try {
                altElement = new AltFindElement(altBaseSettings,altWaitForElementWithTextParameters.getAltFindElementsParameters()).Execute();
                if (altElement != null && altElement.getText().equals(altWaitForElementWithTextParameters.getText())) {
                    return altElement;
                }
            } catch (AltUnityException e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            time += altWaitForElementWithTextParameters.getInterval();
            sleepFor(altWaitForElementWithTextParameters.getInterval());
        }
        throw new WaitTimeOutException("Element with text: " + altWaitForElementWithTextParameters.getText() + " not loaded after " + altWaitForElementWithTextParameters.getTimeout()+ " seconds");
    }
}
