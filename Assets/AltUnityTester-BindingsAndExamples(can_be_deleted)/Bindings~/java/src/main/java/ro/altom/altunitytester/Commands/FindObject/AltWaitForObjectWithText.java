package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

public class AltWaitForObjectWithText extends AltBaseFindObject{
    private AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters;
    public AltWaitForObjectWithText(AltBaseSettings altBaseSettings, AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters) {
        super(altBaseSettings);
        this.altWaitForObjectWithTextParameters = altWaitForObjectWithTextParameters;
    }
    public AltUnityObject Execute(){
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForObjectWithTextParameters.getTimeout()) {
            log.debug("Waiting for element " + altWaitForObjectWithTextParameters.getAltFindObjectsParameters().getValue() + " to have text [" + altWaitForObjectWithTextParameters.getText() + "]");
            try {
                altElement = new AltFindObject(altBaseSettings,altWaitForObjectWithTextParameters.getAltFindObjectsParameters()).Execute();
                if (altElement != null && altElement.getText().equals(altWaitForObjectWithTextParameters.getText())) {
                    return altElement;
                }
            } catch (AltUnityException e) {
                log.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            time += altWaitForObjectWithTextParameters.getInterval();
            sleepFor(altWaitForObjectWithTextParameters.getInterval());
        }
        throw new WaitTimeOutException("Element with text: " + altWaitForObjectWithTextParameters.getText() + " not loaded after " + altWaitForObjectWithTextParameters.getTimeout()+ " seconds");
    }
}
