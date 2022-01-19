package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltBeginTouchParams extends AltMessage {

    private Vector2 coordinates;

    public static class Builder {
        private Vector2 coordinates;

        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;
        }

        public AltBeginTouchParams build() {
            AltBeginTouchParams params = new AltBeginTouchParams();
            params.coordinates = this.coordinates;
            return params;
        }
    }

    private AltBeginTouchParams() {
        this.setCommandName("beginTouch");
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
    }
}
