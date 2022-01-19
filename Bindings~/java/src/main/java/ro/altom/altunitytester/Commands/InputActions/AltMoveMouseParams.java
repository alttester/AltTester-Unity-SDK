package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltMoveMouseParams extends AltMessage {
    public static class Builder {
        private Vector2 coordinates;
        private float duration = 0.1f;
        private boolean wait;

        /**
         * @param coordinates The screen coordinates
         */
        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;
        }

        /**
         * @param duration The time measured in seconds to move the mouse from the
         *                 current position to the set location. Defaults to
         *                 <code>0.1</code>
         */
        public AltMoveMouseParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltMoveMouseParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltMoveMouseParams build() {
            AltMoveMouseParams params = new AltMoveMouseParams();
            params.coordinates = this.coordinates;
            params.duration = this.duration;
            params.wait = this.wait;
            return params;
        }
    }

    private AltMoveMouseParams() {
        this.setCommandName("moveMouse");
    }

    private Vector2 coordinates;
    private float duration;
    private boolean wait;

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 location) {
        this.coordinates = location;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public boolean getWait() {
        return this.wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
