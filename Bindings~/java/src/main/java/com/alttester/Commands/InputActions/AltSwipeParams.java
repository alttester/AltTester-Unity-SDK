/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester.Commands.InputActions;

import com.alttester.AltMessage;
import com.alttester.position.Vector2;

public class AltSwipeParams extends AltMessage {
    public static class Builder {
        private Vector2 start;
        private Vector2 end;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param start Coordinates of the screen where the swipe begins.
         * @param end   Coordinates of the screen where the swipe ends.
         */
        public Builder(Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        }

        /**
         * @param start Coordinates of the screen where the swipe begins.
         */
        public AltSwipeParams.Builder withStart(Vector2 start) {
            this.start = start;
            return this;
        }

        /**
         * @param end Coordinates of the screen where the swipe ends.
         */
        public AltSwipeParams.Builder withEnd(Vector2 end) {
            this.end = end;
            return this;
        }

        /**
         * @param duration The time measured in seconds to move the mouse from start to
         *                 end location
         */
        public AltSwipeParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltSwipeParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltSwipeParams build() {
            AltSwipeParams altTiltParameters = new AltSwipeParams();
            altTiltParameters.start = this.start;
            altTiltParameters.end = this.end;
            altTiltParameters.duration = this.duration;
            altTiltParameters.wait = this.wait;
            return altTiltParameters;
        }
    }

    protected Vector2 start;
    protected Vector2 end;
    protected float duration;
    protected boolean wait;

    protected AltSwipeParams() {
        this.setCommandName("swipe");
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public Vector2 getStart() {
        return start;
    }

    public void setStart(Vector2 start) {
        this.start = start;
    }

    public Vector2 getEnd() {
        return end;
    }

    public void setEnd(Vector2 end) {
        this.end = end;
    }

    public boolean getWait() {
        return this.wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
