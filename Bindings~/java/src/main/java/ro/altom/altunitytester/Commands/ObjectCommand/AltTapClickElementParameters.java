package ro.altom.altunitytester.Commands.ObjectCommand;

public class AltTapClickElementParameters extends AltUnityObjectParameters {
    private int count = 1;
    private float interval = 0.1f;
    private boolean wait = true;

    public static class Builder {
        private int count = 1;
        private float interval = 0.1f;
        private boolean wait = true;

        public Builder() {

        }

        public Builder(int count) {
            this.count = count;
        }

        public Builder(int count, float interval) {
            this.count = count;
            this.interval = interval;
        }

        public AltTapClickElementParameters.Builder withCount(int count) {
            this.count = count;
            return this;
        }

        public AltTapClickElementParameters.Builder withInterval(float interval) {
            this.interval = interval;
            return this;
        }

        public AltTapClickElementParameters.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltTapClickElementParameters build() {
            AltTapClickElementParameters parameters = new AltTapClickElementParameters();
            parameters.count = this.count;
            parameters.interval = this.interval;
            parameters.wait = this.wait;

            return parameters;
        }
    }

    private AltTapClickElementParameters() {
    }

    public int getCount() {
        return this.count;
    }

    public void setCount(int count) {
        this.count = count;
    }

    public float getInterval() {
        return interval;
    }

    public void setInterval(float interval) {
        this.interval = interval;
    }

    public boolean getWait() {
        return wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
