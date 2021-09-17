package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltKeyDown extends AltBaseCommand {

    private AltKeyParameters altKeyDownParameters;

    public AltKeyDown(IMessageHandler messageHandler, AltKeyParameters altKeyDownParameters) {
        super(messageHandler);
        this.altKeyDownParameters = altKeyDownParameters;
    }

    public void Execute() {
        SendCommand(altKeyDownParameters);
        recvall(altKeyDownParameters, String.class);
    }
}
