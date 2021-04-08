package ro.altom.altunitytester.Commands.AltUnityCommands;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltUnitySetServerLogging extends AltBaseCommand {

    private AltSetServerLoggingParameters setServerLoggingParameters;

    public AltUnitySetServerLogging(AltBaseSettings altBaseSettings,
            AltSetServerLoggingParameters setServerLoggingParameters) {
        super(altBaseSettings);

        this.setServerLoggingParameters = setServerLoggingParameters;
    }

    public void Execute() {
        SendCommand("setServerLogging", setServerLoggingParameters.getLogger().toString(),
                setServerLoggingParameters.getLogLevel().toString());
        recvall();

    }

}
