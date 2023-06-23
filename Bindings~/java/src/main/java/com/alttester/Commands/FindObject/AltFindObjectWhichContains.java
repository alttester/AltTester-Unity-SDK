/*
    Copyright(C) 2023  Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

package com.alttester.Commands.FindObject;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;

/**
 * Find the first object in the scene which respects the given criteria.
 */
public class AltFindObjectWhichContains extends AltBaseFindObject {
    private AltFindObjectsParams altFindObjectsParameters;

    public AltFindObjectWhichContains(IMessageHandler messageHandler,
            AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObject");
    }

    public AltObject Execute() {
        altFindObjectsParameters
                .setPath(SetPathContains(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveAltObject(altFindObjectsParameters);
    }
}
