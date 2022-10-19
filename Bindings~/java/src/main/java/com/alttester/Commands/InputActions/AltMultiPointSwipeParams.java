package com.alttester.Commands.InputActions;

import java.util.List;

import com.alttester.AltMessage;
import com.alttester.position.Vector2;

public class AltMultiPointSwipeParams extends AltMessage {
    public static class Builder {
        private List<Vector2> positions;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param positions A list of positions on the screen where the swipe be made.
         */
        public Builder(List<Vector2> positions) {
            this.positions = positions;
        }

        /**
         * @param positions A list of positions on the screen where the swipe be made.
         */
        public AltMultiPointSwipeParams.Builder withPositions(List<Vector2> positions) {
            this.positions = positions;
            return this;
        }

        /**
         * @param duration The time measured in seconds to swipe from first position to
         *                 the last position. Defaults to <code>0.1</code>.
         */
        public AltMultiPointSwipeParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltMultiPointSwipeParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltMultiPointSwipeParams build() {
            AltMultiPointSwipeParams params = new AltMultiPointSwipeParams();
            params.positions = this.positions;
            params.duration = this.duration;
            params.wait = this.wait;
            return params;
        }
    }

    private List<Vector2> positions;
    private float duration;
    private boolean wait;

    private AltMultiPointSwipeParams() {
        this.setCommandName("multipointSwipe");
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public List<Vector2> getPositions() {
        return positions;
    }

    public void setPositions(List<Vector2> positions) {
        this.positions = positions;
    }

    public boolean getWait() {
        return this.wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
