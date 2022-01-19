package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;

public class AltScrollParams extends AltMessage {
    public static class Builder {
        private float speed = 1;
        private float duration = 0.1f;
        private boolean wait = true;

        public Builder() {
        }

        /**
         * 
         * @param duration The duration of the scroll in seconds. Defaults to
         *                 <code> 0.1 </code>
         */
        public AltScrollParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * 
         * @param speed Set how fast to scroll. Positive values will scroll up and
         *              negative values will scroll down. Defaults to <code> 1 </code>
         */
        public AltScrollParams.Builder withSpeed(float speed) {
            this.speed = speed;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltScrollParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltScrollParams build() {
            AltScrollParams altScrollMouseParameters = new AltScrollParams();
            altScrollMouseParameters.speed = this.speed;
            altScrollMouseParameters.duration = this.duration;
            altScrollMouseParameters.wait = this.wait;
            return altScrollMouseParameters;
        }
    }

    private AltScrollParams() {
    }

    private float speed;
    private float duration;
    private boolean wait;

    public float getSpeed() {
        return speed;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
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
