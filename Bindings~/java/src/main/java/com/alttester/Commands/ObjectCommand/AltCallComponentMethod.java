package com.alttester.Commands.ObjectCommand;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

/**
 * Invoke a method from an existing component of the object.
 */
public class AltCallComponentMethod extends AltBaseCommand {

    /**
     * @param altCallComponentMethodParameters builder for calling component methods
     */
    private AltCallComponentMethodParams altCallComponentMethodParameters;

    public AltCallComponentMethod(IMessageHandler messageHandler,
            AltCallComponentMethodParams altCallComponentMethodParameters) {
        super(messageHandler);
        this.altCallComponentMethodParameters = altCallComponentMethodParameters;
        this.altCallComponentMethodParameters.setCommandName("callComponentMethodForObject");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altCallComponentMethodParameters);
        return recvall(altCallComponentMethodParameters, returnType);
    }
}
