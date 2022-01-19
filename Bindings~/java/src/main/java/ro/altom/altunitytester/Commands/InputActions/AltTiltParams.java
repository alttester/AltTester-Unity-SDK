package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector3;

public class AltTiltParams extends AltMessage {
    public static class Builder {
        private Vector3 acceleration;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param acceleration The linear acceleration of a device.
         */
        public Builder(Vector3 acceleration) {
            this.acceleration = acceleration;
        }

        /**
         * @param duration How long the rotation will take in seconds. Defaults to
         *                 <code>0.1<code>.
         */
        public AltTiltParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param acceleration The linear acceleration of a device.
         */
        public AltTiltParams.Builder withAcceleration(Vector3 acceleration) {
            this.acceleration = acceleration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltTiltParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltTiltParams build() {
            AltTiltParams altTiltParameters = new AltTiltParams();
            altTiltParameters.acceleration = this.acceleration;
            altTiltParameters.duration = this.duration;
            altTiltParameters.wait = this.wait;
            return altTiltParameters;
        }
    }

    private AltTiltParams() {
        this.setCommandName("tilt");
    }

    private Vector3 acceleration;
    private float duration;
    private boolean wait;

    public Vector3 getAcceleration() {
        return acceleration;
    }

    public void setAcceleration(Vector3 acceleration) {
        this.acceleration = acceleration;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public boolean getWait() {
        return wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
