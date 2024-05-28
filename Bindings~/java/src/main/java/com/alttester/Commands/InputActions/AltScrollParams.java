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

public class AltScrollParams extends AltMessage {
    public static class Builder {
        private float speed = 1;
        private float speedHorizontal = 1;
        private float duration = 0.1f;
        private boolean wait = true;

        public Builder() {
        }

        /**
         * 
         * @param duration The duration of the scroll in seconds. Defaults to
         *                 <code> 0.1 </code>
         */
        public AltScrollParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * 
         * @param speed Set how fast to scroll. Positive values will scroll up and
         *              negative values will scroll down. Defaults to <code> 1 </code>
         */
        public AltScrollParams.Builder withSpeed(float speed) {
            this.speed = speed;
            return this;
        }

        /**
         * 
         * @param speed Set how fast to scroll right or left. Defaults to
         *              <code> 1 </code>
         */
        public AltScrollParams.Builder withHorizontalSpeed(float speed) {
            this.speedHorizontal = speed;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltScrollParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltScrollParams build() {
            AltScrollParams altScrollMouseParameters = new AltScrollParams();
            altScrollMouseParameters.speed = this.speed;
            altScrollMouseParameters.duration = this.duration;
            altScrollMouseParameters.wait = this.wait;
            altScrollMouseParameters.setSpeedHorizontal(this.speedHorizontal);
            return altScrollMouseParameters;
        }
    }

    private AltScrollParams() {
    }

    public float getSpeedHorizontal() {
        return speedHorizontal;
    }

    public void setSpeedHorizontal(float speedHorizontal) {
        this.speedHorizontal = speedHorizontal;
    }

    private float speed;
    private float duration;
    private boolean wait;
    private float speedHorizontal;

    public float getSpeed() {
        return speed;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
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
