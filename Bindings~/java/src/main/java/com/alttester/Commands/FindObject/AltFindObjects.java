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

package com.alttester.Commands.FindObject;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;

/**
 * Builder for finding all objects in the scene that respect the given criteria.
 * It's no longer possible to search for object by name giving a path in the
 * hierarchy. For searching by name, use searching by path.
 */
public class AltFindObjects extends AltBaseFindObject {
    private AltFindObjectsParams altFindObjectsParameters;

    /**
     * @param altFindObjectsParameters the properties parameter for finding it in a
     *                                 scene.
     * @param messageHandler
     */
    public AltFindObjects(IMessageHandler messageHandler, AltFindObjectsParams altFindObjectsParameters) {
        super(messageHandler);
        this.altFindObjectsParameters = altFindObjectsParameters;
        this.altFindObjectsParameters.setCommandName("findObjects");
    }

    public AltObject[] Execute() {
        altFindObjectsParameters
                .setPath(SetPath(altFindObjectsParameters.getBy(), altFindObjectsParameters.getValue()));
        altFindObjectsParameters.setCameraPath(
                SetPath(altFindObjectsParameters.getCameraBy(), altFindObjectsParameters.getCameraValue()));
        SendCommand(altFindObjectsParameters);
        return ReceiveListOfAltObjects(altFindObjectsParameters);
    }
}
