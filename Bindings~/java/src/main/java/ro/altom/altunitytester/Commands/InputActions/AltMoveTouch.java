package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

public class AltMoveTouch extends AltBaseCommand {
    private Vector2 coordinates;
    private int fingerId;

    public AltMoveTouch(AltBaseSettings altBaseSettings, int fingerId, Vector2 coordinates) {
        super(altBaseSettings);
        this.coordinates = coordinates;
        this.fingerId = fingerId;
    }

    public void Execute() {
        String position = vectorToJsonString(coordinates.x, coordinates.y);
        SendCommand("moveTouch", String.valueOf(fingerId), position);
        String data = recvall();
        validateResponse("Ok", data);
    }
}
