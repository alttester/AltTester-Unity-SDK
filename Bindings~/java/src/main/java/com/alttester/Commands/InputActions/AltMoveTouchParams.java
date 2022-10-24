package com.alttester.Commands.InputActions;

import com.alttester.AltMessage;
import com.alttester.position.Vector2;

public class AltMoveTouchParams extends AltMessage {

    private int fingerId;
    private Vector2 coordinates;

    public static class Builder {
        private int fingerId;
        private Vector2 coordinates;

        public Builder(int fingerId, Vector2 coordinates) {
            this.fingerId = fingerId;
            this.coordinates = coordinates;
        }

        public AltMoveTouchParams build() {
            AltMoveTouchParams params = new AltMoveTouchParams();
            params.fingerId = this.fingerId;
            params.coordinates = this.coordinates;
            return params;
        }
    }

    private AltMoveTouchParams() {
        this.setCommandName("moveTouch");
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
    }

    public int getFingerId() {
        return fingerId;
    }

    public void setFingerId(int fingerId) {
        this.fingerId = fingerId;
    }
}
