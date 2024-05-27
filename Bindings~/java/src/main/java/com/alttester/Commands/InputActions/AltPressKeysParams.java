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
import com.alttester.UnityStruct.AltKeyCode;

public class AltPressKeysParams extends AltMessage {
    public static class Builder {
        private AltKeyCode[] keyCodes;
        private float power = 1;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param keyCodes The key code of the key simulated to be pressed.
         */
        public Builder(AltKeyCode[] keyCodes) {
            this.keyCodes = keyCodes;
        }

        /**
         * 
         * @param duration The time measured in seconds from the key press to the key
         *                 release. Defaults to <code>0.1</code>
         */
        public AltPressKeysParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * 
         * @param power A value between [-1,1] used for joysticks to indicate how hard
         *              the button was pressed. Defaults to <code>1</code>
         */
        public AltPressKeysParams.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         * @return current object
         */
        public AltPressKeysParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltPressKeysParams build() {
            AltPressKeysParams altPressKeyParameters = new AltPressKeysParams();
            altPressKeyParameters.keyCodes = this.keyCodes;
            altPressKeyParameters.power = this.power;
            altPressKeyParameters.duration = this.duration;
            altPressKeyParameters.wait = this.wait;
            return altPressKeyParameters;
        }
    }

    private AltPressKeysParams() {
    }

    private AltKeyCode[] keyCodes;
    private float power;
    private float duration;
    private boolean wait;

    public AltKeyCode[] getKeyCodes() {
        return keyCodes;
    }

    public void setKeyCodes(AltKeyCode[] keyCodes) {
        this.keyCodes = keyCodes;
    }

    public float getPower() {
        return power;
    }

    public void setPower(float power) {
        this.power = power;
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
