package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Invoke a method from an existing component of the object.
 */
public class AltCallComponentMethod extends AltBaseCommand {

    /**
     * @param altCallComponentMethodParameters builder for calling component methods
     */
    private AltCallComponentMethodParameters altCallComponentMethodParameters;

    public AltCallComponentMethod(IMessageHandler messageHandler,
            AltCallComponentMethodParameters altCallComponentMethodParameters) {
        super(messageHandler);
        this.altCallComponentMethodParameters = altCallComponentMethodParameters;
        this.altCallComponentMethodParameters.setCommandName("callComponentMethodForObject");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altCallComponentMethodParameters);
        return recvall(altCallComponentMethodParameters, returnType);
    }
}
