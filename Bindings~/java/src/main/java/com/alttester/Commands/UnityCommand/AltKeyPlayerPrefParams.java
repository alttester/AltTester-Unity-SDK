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

package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.AltDriver;

public class AltKeyPlayerPrefParams extends AltMessage {

    private String keyName;
    private int keyType;

    AltKeyPlayerPrefParams(String keyName) {
        this.setKeyName(keyName);
    }

    AltKeyPlayerPrefParams(String keyName, AltDriver.PlayerPrefsKeyType keyType) {
        this.keyName = keyName;
        this.keyType = keyType.getVal();
    }

    public int getType() {
        return keyType;
    }

    public void setType(AltDriver.PlayerPrefsKeyType keyType) {
        this.keyType = keyType.getVal();
    }

    public String getKeyName() {
        return keyName;
    }

    public void setKeyName(String keyName) {
        this.keyName = keyName;
    }
}
