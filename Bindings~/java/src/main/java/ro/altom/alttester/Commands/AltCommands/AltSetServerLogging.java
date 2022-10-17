package ro.altom.alttester.Commands.AltCommands;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

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
