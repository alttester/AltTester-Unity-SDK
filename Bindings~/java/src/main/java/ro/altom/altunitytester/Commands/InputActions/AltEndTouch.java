package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltEndTouch extends AltBaseCommand {
    private AltEndTouchParameters params;

    public AltEndTouch(IMessageHandler messageHandler, int fingerId) {
        super(messageHandler);
        params = new AltEndTouchParameters(fingerId);
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
