package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.altUnityTesterExceptions.AltUnityException;
import ro.altom.altunitytester.altUnityTesterExceptions.WaitTimeOutException;

/**
 * Wait until it finds an object that respect the given criteria and it has the
 * text you are looking for or times run out and will throw an error.
 */
public class AltWaitForObjectWithText extends AltBaseFindObject {

    /**
     * @param altWaitForObjectWithTextParameters the properties parameter for
     *                                           finding the objects in a scene.
     */
    private AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters;

    public AltWaitForObjectWithText(IMessageHandler messageHandler,
            AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters) {
        super(messageHandler);
        this.altWaitForObjectWithTextParameters = altWaitForObjectWithTextParameters;
    }

    public AltUnityObject Execute() {
        double time = 0;
        AltUnityObject altElement = null;
        while (time < altWaitForObjectWithTextParameters.getTimeout()) {
            logger.debug(
                    "Waiting for element " + altWaitForObjectWithTextParameters.getAltFindObjectsParameters().getPath()
                            + " to have text [" + altWaitForObjectWithTextParameters.getText() + "]");
            try {
                altElement = new AltFindObject(messageHandler,
                        altWaitForObjectWithTextParameters.getAltFindObjectsParameters()).Execute();
                if (altElement != null && altElement.getText().equals(altWaitForObjectWithTextParameters.getText())) {
                    return altElement;
                }
            } catch (AltUnityException e) {
                logger.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            time += altWaitForObjectWithTextParameters.getInterval();
            sleepFor(altWaitForObjectWithTextParameters.getInterval());
        }
        throw new WaitTimeOutException("Element with text: " + altWaitForObjectWithTextParameters.getText()
                + " not loaded after " + altWaitForObjectWithTextParameters.getTimeout() + " seconds");
    }
}
