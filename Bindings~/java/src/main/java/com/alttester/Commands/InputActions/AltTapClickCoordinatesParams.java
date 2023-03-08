package com.alttester.Commands.InputActions;

import com.alttester.AltMessage;
import com.alttester.Position.Vector2;

public class AltTapClickCoordinatesParams extends AltMessage {
    private Vector2 coordinates;
    private int count = 1;
    private float interval = 0.1f;
    private boolean wait = true;

    public static class Builder {
        private Vector2 coordinates;
        private int count = 1;
        private float interval = 0.1f;
        private boolean wait = true;

        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;
        }

        public Builder(Vector2 coordinates, int count) {
            this.coordinates = coordinates;
            this.count = count;
        }

        public Builder(Vector2 coordinates, int count, float interval) {
            this.coordinates = coordinates;
            this.count = count;
            this.interval = interval;
        }

        public AltTapClickCoordinatesParams.Builder withCount(int count) {
            this.count = count;
            return this;
        }

        public AltTapClickCoordinatesParams.Builder withInterval(float interval) {
            this.interval = interval;
            return this;
        }

        public AltTapClickCoordinatesParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltTapClickCoordinatesParams build() {
            AltTapClickCoordinatesParams parameters = new AltTapClickCoordinatesParams();
            parameters.coordinates = this.coordinates;
            parameters.count = this.count;
            parameters.interval = this.interval;
            parameters.wait = this.wait;

            return parameters;
        }
    }

    private AltTapClickCoordinatesParams() {
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
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
