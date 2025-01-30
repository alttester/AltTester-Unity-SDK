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
import com.alttester.position.Vector3;

public class AltTiltParams extends AltMessage {
    public static class Builder {
        private Vector3 acceleration;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param acceleration The linear acceleration of a device.
         */
        public Builder(Vector3 acceleration) {
            this.acceleration = acceleration;
        }

        /**
         * @param duration How long the rotation will take in seconds. Defaults to 0.1
         */
        public AltTiltParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * @param acceleration The linear acceleration of a device.
         */
        public AltTiltParams.Builder withAcceleration(Vector3 acceleration) {
            this.acceleration = acceleration;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltTiltParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltTiltParams build() {
            AltTiltParams altTiltParameters = new AltTiltParams();
            altTiltParameters.acceleration = this.acceleration;
            altTiltParameters.duration = this.duration;
            altTiltParameters.wait = this.wait;
            return altTiltParameters;
        }
    }

    private AltTiltParams() {
        this.setCommandName("tilt");
    }

    private Vector3 acceleration;
    private float duration;
    private boolean wait;

    public Vector3 getAcceleration() {
        return acceleration;
    }

    public void setAcceleration(Vector3 acceleration) {
        this.acceleration = acceleration;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public boolean getWait() {
        return wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }
}
