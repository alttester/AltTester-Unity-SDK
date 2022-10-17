package ro.altom.alttester.Commands;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;

/**
 * Get the value of a property from one of the component of the object.
 */
public class AltGetStaticProperty extends AltBaseCommand {
    /**
     * @param altGetComponentPropertyParameters builder for getting components'
     *                                          property
     */
    private AltGetComponentPropertyParams altGetComponentPropertyParameters;

    public AltGetStaticProperty(IMessageHandler messageHandler,
            AltGetComponentPropertyParams altGetComponentPropertyParameters) {
        super(messageHandler);
        this.altGetComponentPropertyParameters = altGetComponentPropertyParameters;
        altGetComponentPropertyParameters.setAltObject(null);
        this.altGetComponentPropertyParameters.setCommandName("getObjectComponentProperty");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altGetComponentPropertyParameters);
        return recvall(altGetComponentPropertyParameters, returnType);
    }
}