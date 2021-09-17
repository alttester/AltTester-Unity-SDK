package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltMoveTouchParameters extends AltMessage{
    
    private int fingerId;
    private Vector2 coordinates;

    AltMoveTouchParameters(int fingerId, Vector2 coordinates){
        this.setCommandName("moveTouch");
        this.setFingerId(fingerId);
        this.setCoordinates(coordinates);
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
    }

    public int getFingerId() {
        return fingerId;
    }

    public void setFingerId(int fingerId) {
        this.fingerId = fingerId;
    }
}
