package com.alttester.Commands.ObjectCommand;

public class AltTapClickElementParams extends AltObjectParams {
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

        public AltTapClickElementParams.Builder withCount(int count) {
            this.count = count;
            return this;
        }

        public AltTapClickElementParams.Builder withInterval(float interval) {
            this.interval = interval;
            return this;
        }

        public AltTapClickElementParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltTapClickElementParams build() {
            AltTapClickElementParams parameters = new AltTapClickElementParams();
            parameters.count = this.count;
            parameters.interval = this.interval;
            parameters.wait = this.wait;

            return parameters;
        }
    }

    private AltTapClickElementParams() {
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
