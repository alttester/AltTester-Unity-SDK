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
