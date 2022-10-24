package com.alttester.Commands.InputActions;

import com.alttester.AltMessage;

public class AltEndTouchParams extends AltMessage {

    private int fingerId;

    public static class Builder {
        private int fingerId;

        public Builder(int fingerId) {
            this.fingerId = fingerId;
        }

        public AltEndTouchParams build() {
            AltEndTouchParams params = new AltEndTouchParams();
            params.fingerId = this.fingerId;
            return params;
        }
    }

    private AltEndTouchParams() {
        this.setCommandName("endTouch");
    }

    public int getFingerId() {
        return fingerId;
    }

    public void setFingerId(int fingerId) {
        this.fingerId = fingerId;
    }
}
