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

public class AltKeysUpParams extends AltMessage {

    private AltKeyCode[] keyCodes;

    public static class Builder {
        private AltKeyCode[] keyCodes;

        public Builder(AltKeyCode[] keyCodes) {
            this.keyCodes = keyCodes;
        }

        public AltKeysUpParams build() {
            AltKeysUpParams params = new AltKeysUpParams();
            params.keyCodes = this.keyCodes;
            return params;
        }
    }

    private AltKeysUpParams() {
        this.setCommandName("keysUp");
    }

    public AltKeyCode[] getKeyCode() {
        return keyCodes;
    }

    public void setKeyCode(AltKeyCode[] keyCodes) {
        this.keyCodes = keyCodes;
    }
}
