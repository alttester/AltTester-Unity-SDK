package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;

public class AltTiltParameters extends AltMessage{
    public static class Builder {
        private int x = 0;
        private int y = 0;
        private int z = 0;
        private float duration = 0;

        public Builder(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public AltTiltParameters.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        public AltTiltParameters build() {
            AltTiltParameters altTiltParameters = new AltTiltParameters();
            altTiltParameters.x = this.x;
            altTiltParameters.y = this.y;
            altTiltParameters.z = this.z;
            altTiltParameters.duration = this.duration;
            return altTiltParameters;
        }
    }

    private AltTiltParameters() {
        this.setCommandName("tilt");
    }

    private int x;
    private int y;
    private int z;
    private float duration;

    public int getX() {
        return x;
    }

    public void setX(int x) {
        this.x = x;
    }

    public int getY() {
        return y;
    }

    public void setY(int y) {
        this.y = y;
    }

    public int getZ() {
        return z;
    }

    public void setZ(int z) {
        this.z = z;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

}
