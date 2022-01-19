package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltKeyDown extends AltBaseCommand {

    private AltKeyDownParams altKeyDownParameters;

    public AltKeyDown(IMessageHandler messageHandler, AltKeyDownParams altKeyDownParameters) {
        super(messageHandler);
        this.altKeyDownParameters = altKeyDownParameters;
    }

    public void Execute() {
        SendCommand(altKeyDownParameters);
        recvall(altKeyDownParameters, String.class);
    }
}
