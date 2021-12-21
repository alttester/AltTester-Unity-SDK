package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Get the value of a property from one of the component of the object.
 */
public class AltGetComponentProperty extends AltBaseCommand {
    /**
     * @param altGetComponentPropertyParameters builder for getting components'
     *                                          property
     */
    private AltGetComponentPropertyParameters altGetComponentPropertyParameters;

    public AltGetComponentProperty(IMessageHandler messageHandler,
            AltGetComponentPropertyParameters altGetComponentPropertyParameters) {
        super(messageHandler);
        this.altGetComponentPropertyParameters = altGetComponentPropertyParameters;
        this.altGetComponentPropertyParameters.setCommandName("getObjectComponentProperty");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altGetComponentPropertyParameters);
        return recvall(altGetComponentPropertyParameters, returnType);
    }
}
