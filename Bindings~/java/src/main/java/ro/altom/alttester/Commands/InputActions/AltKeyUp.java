package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltKeyUp extends AltBaseCommand {

    private AltKeyUpParams params;

    public AltKeyUp(IMessageHandler messageHandler, AltKeyUpParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        recvall(params, String.class);
    }
}
