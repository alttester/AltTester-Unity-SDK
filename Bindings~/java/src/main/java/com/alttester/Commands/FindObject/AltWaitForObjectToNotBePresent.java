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

import com.alttester.Utils;
import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.altTesterExceptions.AltException;

/**
 * Wait until the object in the scene that respect the given criteria is no
 * longer in the scene or times run out and will throw an error.
 */
public class AltWaitForObjectToNotBePresent extends AltBaseFindObject {

    /**
     * @param altWaitForObjectsParameters the properties parameter for finding the
     *                                    objects in a scene.
     */
    private AltWaitForObjectsParams altWaitForObjectsParameters;

    public AltWaitForObjectToNotBePresent(IMessageHandler messageHandler,
            AltWaitForObjectsParams altWaitForObjectsParameters) {
        super(messageHandler);
        this.altWaitForObjectsParameters = altWaitForObjectsParameters;
    }

    public void Execute() {
        double time = 0;
        AltObject altElement = null;
        while (time <= altWaitForObjectsParameters.getTimeout()) {
            altElement = null;
            logger.debug("Waiting for element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath()
                    + " not to be present");
            try {
                altElement = new AltFindObject(messageHandler,
                        altWaitForObjectsParameters.getAltFindObjectsParameters()).Execute();
                if (altElement == null) {
                    return;
                }
            } catch (Exception e) {
                logger.warn(e.getLocalizedMessage());
                break;
            }
            Utils.sleepFor(altWaitForObjectsParameters.getInterval());
            time += altWaitForObjectsParameters.getInterval();
        }

        if (altElement != null) {
            throw new AltException(
                    "Element " + altWaitForObjectsParameters.getAltFindObjectsParameters().getPath()
                            + " still found after " + altWaitForObjectsParameters.getTimeout() + " seconds");
        }
    }
}
