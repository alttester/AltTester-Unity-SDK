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

package com.alttester.Commands.FindObject;

import com.alttester.IMessageHandler;
import com.alttester.AltDriver;
import com.alttester.Commands.AltCommandReturningAltObjects;

public class AltBaseFindObject extends AltCommandReturningAltObjects {
    public AltBaseFindObject(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    protected String SetPath(AltDriver.By by, String value) {
        String path = "";
        switch (by) {
            case TAG:
                path = "//*[@tag=" + value + "]";
                break;
            case LAYER:
                path = "//*[@layer=" + value + "]";
                break;
            case NAME:
                path = "//" + value;
                break;
            case COMPONENT:
                path = "//*[@component=" + value + "]";
                break;
            case PATH:
                path = value;
                break;
            case ID:
                path = "//*[@id=" + value + "]";
                break;
            case TEXT:
                path = "//*[@text=" + value + "]";
                break;
        }
        return path;
    }

    protected String SetPathContains(AltDriver.By by, String value) {
        String path = "";
        switch (by) {
            case TAG:
                path = "//*[contains(@tag," + value + ")]";
                break;
            case LAYER:
                path = "//*[contains(@layer," + value + ")]";
                break;
            case NAME:
                path = "//*[contains(@name," + value + ")]";
                break;
            case COMPONENT:
                path = "//*[contains(@component," + value + ")]";
                break;
            case PATH:
                path = value;
                break;
            case ID:
                path = "//*[contains(@id," + value + ")]";
                break;
            case TEXT:
                path = "//*[contains(@text," + value + ")]";
                break;
        }
        return path;
    }
}
