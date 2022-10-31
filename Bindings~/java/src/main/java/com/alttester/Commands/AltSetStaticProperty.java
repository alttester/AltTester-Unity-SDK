package com.alttester.Commands;

import com.alttester.IMessageHandler;
import com.alttester.Commands.ObjectCommand.AltSetComponentPropertyParams;

/**
 * Get the value of a property from one of the component of the object.
 */
public class AltSetStaticProperty extends AltBaseCommand {
    /**
     * @param altSetComponentPropertyParameters builder for setting components'
     *                                          property
     */
    private AltSetComponentPropertyParams altSetComponentPropertyParameters;

    public AltSetStaticProperty(IMessageHandler messageHandler,
            AltSetComponentPropertyParams altSetComponentPropertyParameters) {
        super(messageHandler);
        this.altSetComponentPropertyParameters = altSetComponentPropertyParameters;
        altSetComponentPropertyParameters.setAltObject(null);
        this.altSetComponentPropertyParameters.setCommandName("getObjectComponentProperty");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altSetComponentPropertyParameters);
        return recvall(altSetComponentPropertyParameters, returnType);
    }
}