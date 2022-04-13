package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltKeysUp extends AltBaseCommand {

    private AltKeysUpParams params;

    public AltKeysUp(IMessageHandler messageHandler, AltKeysUpParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        recvall(params, String.class);
    }
}
