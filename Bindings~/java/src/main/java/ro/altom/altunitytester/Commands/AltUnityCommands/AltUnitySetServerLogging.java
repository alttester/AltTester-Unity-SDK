package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltUnitySetServerLogging extends AltBaseCommand {

    private AltSetServerLoggingParams setServerLoggingParameters;

    public AltUnitySetServerLogging(IMessageHandler messageHandler,
            AltSetServerLoggingParams setServerLoggingParameters) {
        super(messageHandler);

        this.setServerLoggingParameters = setServerLoggingParameters;
    }

    public void Execute() {
        SendCommand(setServerLoggingParameters);
        recvall(setServerLoggingParameters, String.class);

    }

}
