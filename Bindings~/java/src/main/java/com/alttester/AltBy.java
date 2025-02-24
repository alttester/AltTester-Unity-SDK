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

package com.alttester;

import com.alttester.AltDriver.By;
import lombok.Getter;


@Getter
public class AltBy {

    public By by;
    public String value;

    public AltBy(By by, String value) {
        this.by = by;
        this.value = value;
    }

    public static AltBy id(String value) {
        return new AltBy(By.ID, value);
    }

    public static AltBy name(String value) {
        return new AltBy(By.NAME, value);
    }

    public static AltBy path(String value) {
        return new AltBy(By.PATH, value);
    }

    public static AltBy tag(String value) {
        return new AltBy(By.TAG, value);
    }

    public static AltBy text(String value) {
        return new AltBy(By.TEXT, value);
    }

    public static AltBy component(String value) {
        return new AltBy(By.COMPONENT, value);
    }

    public static AltBy layer(String value) {
        return new AltBy(By.LAYER, value);
    }
}
