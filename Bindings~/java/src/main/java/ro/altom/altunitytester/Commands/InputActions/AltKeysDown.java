package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltKeysDown extends AltBaseCommand {

    private AltKeysDownParams altKeysDownParameters;

    public AltKeysDown(IMessageHandler messageHandler, AltKeysDownParams altKeysDownParameters) {
        super(messageHandler);
        this.altKeysDownParameters = altKeysDownParameters;
    }

    public void Execute() {
        SendCommand(altKeysDownParameters);
        recvall(altKeysDownParameters, String.class);
    }
}
