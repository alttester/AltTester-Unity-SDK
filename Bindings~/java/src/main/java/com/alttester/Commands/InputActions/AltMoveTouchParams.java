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

public class AltMoveTouchParams extends AltMessage {

    private int fingerId;
    private Vector2 coordinates;

    public static class Builder {
        private int fingerId;
        private Vector2 coordinates;

        public Builder(int fingerId, Vector2 coordinates) {
            this.fingerId = fingerId;
            this.coordinates = coordinates;
        }

        public AltMoveTouchParams build() {
            AltMoveTouchParams params = new AltMoveTouchParams();
            params.fingerId = this.fingerId;
            params.coordinates = this.coordinates;
            return params;
        }
    }

    private AltMoveTouchParams() {
        this.setCommandName("moveTouch");
    }

    public Vector2 getCoordinates() {
        return coordinates;
    }

    public void setCoordinates(Vector2 coordinates) {
        this.coordinates = coordinates;
    }

    public int getFingerId() {
        return fingerId;
    }

    public void setFingerId(int fingerId) {
        this.fingerId = fingerId;
    }
}
