package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.ObjectCommand.AltGetComponentPropertyParams;

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
        altGetComponentPropertyParameters.setAltUnityObject(null);
        this.altGetComponentPropertyParameters.setCommandName("getObjectComponentProperty");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altGetComponentPropertyParameters);
        return recvall(altGetComponentPropertyParameters, returnType);
    }
}