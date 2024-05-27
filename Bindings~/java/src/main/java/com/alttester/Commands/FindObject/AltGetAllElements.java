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
import com.alttester.AltObject;
import com.alttester.AltDriver.By;

/**
 * Returns information about every object loaded in the currently loaded scenes.
 */
public class AltGetAllElements extends AltBaseFindObject {
    /**
     * @param altGetAllElementsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    private AltGetAllElementsParams altGetAllElementsParameters;

    public AltGetAllElements(IMessageHandler messageHandler, AltGetAllElementsParams altGetAllElementsParameters) {
        super(messageHandler);
        this.altGetAllElementsParameters = altGetAllElementsParameters;
        this.altGetAllElementsParameters.setCommandName("findObjects");
    }

    public AltObject[] Execute() {
        AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(By.PATH, "//*")
                .withCamera(altGetAllElementsParameters.getCameraBy(), altGetAllElementsParameters.getCameraValue())
                .build();
        return new AltFindObjects(messageHandler, altFindObjectsParameters).Execute();
    }
}
