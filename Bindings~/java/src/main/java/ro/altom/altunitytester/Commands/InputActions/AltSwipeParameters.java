package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltSwipeParameters extends AltMessage {
    public static class Builder {
        private Vector2 start;
        private Vector2 end;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param start Coordinates of the screen where the swipe begins.
         * @param end   Coordinates of the screen where the swipe ends.
         */
        public Builder(Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        }

        /**
         * @param start Coordinates of the screen where the swipe begins.
         */
        public AltSwipeParameters.Builder withStart(Vector2 start) {
            this.start = start;
            return this;
        }

        /**
         * @param end Coordinates of the screen where the swipe ends.
         */
        public AltSwipeParameters.Builder withEnd(Vector2 end) {
            this.end = end;
            return this;
        }

        /**
         * @param duration The time measured in seconds to move the mouse from start to
         *                 end location
         */
        public AltSwipeParameters.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltSwipeParameters.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltSwipeParameters build() {
            AltSwipeParameters altTiltParameters = new AltSwipeParameters();
            altTiltParameters.start = this.start;
            altTiltParameters.end = this.end;
            altTiltParameters.duration = this.duration;
            altTiltParameters.wait = this.wait;
            return altTiltParameters;
        }
    }

    protected Vector2 start;
    protected Vector2 end;
    protected float duration;
    protected boolean wait;

    protected AltSwipeParameters() {
        this.setCommandName("swipe");
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public Vector2 getStart() {
        return start;
    }

    public void setStart(Vector2 start) {
        this.start = start;
    }

    public Vector2 getEnd() {
        return end;
    }

    public void setEnd(Vector2 end) {
        this.end = end;
    }

    public boolean getWait() {
        return this.wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
