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

import com.alttester.position.Vector2;

public class AltHoldParams extends AltSwipeParams {
    public static class Builder {
        private Vector2 coordinates;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param coordinates The coordinates where the button is held down.
         */
        public Builder(Vector2 coordinates) {
            this.coordinates = coordinates;

        }

        /**
         * @param coordinates The coordinates where the button is held down.
         */
        public AltHoldParams.Builder withCoordinates(Vector2 coordinates) {
            this.coordinates = coordinates;
            return this;
        }

        /**
         * @param duration The time measured in seconds to keep the button down.
         */
        public AltHoldParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltHoldParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltHoldParams build() {
            AltHoldParams altTiltParameters = new AltHoldParams();
            altTiltParameters.start = this.coordinates;
            altTiltParameters.end = this.coordinates;
            altTiltParameters.duration = this.duration;
            altTiltParameters.wait = this.wait;
            return altTiltParameters;
        }
    }
}
