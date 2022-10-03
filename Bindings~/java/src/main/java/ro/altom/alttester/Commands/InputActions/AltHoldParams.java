package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.position.Vector2;

public class AltHoldParams extends AltSwipeParams {
    public static class Builder {
        private Vector2 coordinates;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param coordinates The coordinates where the button is held down.
         * @param duration    The time measured in seconds to hold
         */
        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;

        }

        /**
         * @param start The coordinates where the button is held down.
         */
        public AltHoldParams.Builder withCoordinates(Vector2 coordinates) {
            this.coordinates = coordinates;
            return this;
        }

        /**
         * @param duration The time measured in seconds to keep the button down.
         */
        public AltHoldParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltHoldParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltHoldParams build() {
            AltHoldParams altTiltParameters = new AltHoldParams();
            altTiltParameters.start = this.coordinates;
            altTiltParameters.end = this.coordinates;
            altTiltParameters.duration = this.duration;
            altTiltParameters.wait = this.wait;
            return altTiltParameters;
        }
    }
}