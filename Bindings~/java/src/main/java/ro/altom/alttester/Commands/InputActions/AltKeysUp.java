package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

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
