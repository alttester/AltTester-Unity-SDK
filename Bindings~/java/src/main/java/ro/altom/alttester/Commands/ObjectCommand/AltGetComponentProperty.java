package ro.altom.alttester.Commands.ObjectCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

/**
 * Get the value of a property from one of the component of the object.
 */
public class AltGetComponentProperty extends AltBaseCommand {
    /**
     * @param altGetComponentPropertyParameters builder for getting components'
     *                                          property
     */
    private AltGetComponentPropertyParams altGetComponentPropertyParameters;

    public AltGetComponentProperty(IMessageHandler messageHandler,
            AltGetComponentPropertyParams altGetComponentPropertyParameters) {
        super(messageHandler);
        this.altGetComponentPropertyParameters = altGetComponentPropertyParameters;
        this.altGetComponentPropertyParameters.setCommandName("getObjectComponentProperty");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altGetComponentPropertyParameters);
        return recvall(altGetComponentPropertyParameters, returnType);
    }
}
