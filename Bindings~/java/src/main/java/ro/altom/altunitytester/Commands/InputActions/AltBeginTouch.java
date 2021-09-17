package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

public class AltBeginTouch extends AltBaseCommand {
    private AltBeginTouchParameters params;

    public AltBeginTouch(IMessageHandler messageHandler, Vector2 coordinates) {
        super(messageHandler);
        params = new AltBeginTouchParameters(coordinates);
    }

    public int Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        return Integer.parseInt(data);
    }
}
