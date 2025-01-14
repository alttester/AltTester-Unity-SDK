/*
    Copyright(C) 2025 Altom Consulting

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

public class AltMoveMouseParams extends AltMessage {
    public static class Builder {
        private Vector2 coordinates;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param coordinates The screen coordinates
         */
        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;
        }

        /**
         * @param duration The time measured in seconds to move the mouse from the
         *                 current position to the set location. Defaults to
         *                 <code>0.1</code>
         */
        public AltMoveMouseParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltMoveMouseParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltMoveMouseParams build() {
            AltMoveMouseParams params = new AltMoveMouseParams();
            params.coordinates = this.coordinates;
            params.duration = this.duration;
            params.wait = this.wait;
            return params;
        }
    }

    private AltMoveMouseParams() {
        this.setCommandName("moveMouse");
    }

    private Vector2 coordinates;
    private float duration;
    private boolean wait;

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 location) {
        this.coordinates = location;
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
