package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltBeginTouchParameters extends AltMessage{
    
    private Vector2 coordinates;

    public AltBeginTouchParameters(Vector2 coordinates){
        this.setCommandName("beginTouch");
        this.setCoordinates(coordinates);
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
    }
}
