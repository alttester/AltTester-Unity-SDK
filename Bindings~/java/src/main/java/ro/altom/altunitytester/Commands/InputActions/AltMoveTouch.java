package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

public class AltMoveTouch extends AltBaseCommand {
    private AltMoveTouchParameters params;

    public AltMoveTouch(IMessageHandler messageHandler, int fingerId, Vector2 coordinates) {
        super(messageHandler);
        params = new AltMoveTouchParameters(fingerId, coordinates);
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
