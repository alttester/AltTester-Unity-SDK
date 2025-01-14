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
import com.alttester.UnityStruct.AltKeyCode;

public class AltKeysDownParams extends AltMessage {
    public static class Builder {
        private AltKeyCode[] keyCodes;
        private float power = 1;

        public Builder(AltKeyCode[] keyCodes) {
            this.keyCodes = keyCodes;
        }

        public AltKeysDownParams.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        public AltKeysDownParams build() {
            AltKeysDownParams params = new AltKeysDownParams();
            params.keyCodes = this.keyCodes;
            params.power = this.power;
            return params;
        }
    }

    private AltKeysDownParams() {
        this.setCommandName("keysDown");
    }

    private AltKeyCode[] keyCodes;
    private float power;

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

}
