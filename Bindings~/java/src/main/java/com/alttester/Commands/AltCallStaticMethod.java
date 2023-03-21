package com.alttester.Commands;

import com.alttester.IMessageHandler;

public class AltCallStaticMethod extends AltBaseCommand {
    AltCallStaticMethodParams altCallStaticMethodParameters;

    public AltCallStaticMethod(IMessageHandler messageHandler,
            AltCallStaticMethodParams altCallStaticMethodParameters) {
        super(messageHandler);
        this.altCallStaticMethodParameters = altCallStaticMethodParameters;
        this.altCallStaticMethodParameters.setCommandName("callComponentMethodForObject");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altCallStaticMethodParameters);
        return recvall(altCallStaticMethodParameters, returnType);
    }
}
