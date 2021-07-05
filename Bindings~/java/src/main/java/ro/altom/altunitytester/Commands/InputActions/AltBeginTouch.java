package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

public class AltBeginTouch extends AltBaseCommand {
    private Vector2 coordinates;

    public AltBeginTouch(AltBaseSettings altBaseSettings, Vector2 coordinates) {
        super(altBaseSettings);
        this.coordinates = coordinates;
    }

    public int Execute() {
        String position = vectorToJsonString(coordinates.x, coordinates.y);
        SendCommand("beginTouch", position);
        String data = recvall();
        return Integer.parseInt(data);
    }
}
