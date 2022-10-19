package com.alttester.Commands.AltCommands;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltSetServerLogging extends AltBaseCommand {

    private AltSetServerLoggingParams setServerLoggingParameters;

    public AltSetServerLogging(IMessageHandler messageHandler,
            AltSetServerLoggingParams setServerLoggingParameters) {
        super(messageHandler);

        this.setServerLoggingParameters = setServerLoggingParameters;
    }

    public void Execute() {
        SendCommand(setServerLoggingParameters);
        recvall(setServerLoggingParameters, String.class);

    }

}
