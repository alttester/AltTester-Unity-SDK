/*
    Copyright(C) 2023 Altom Consulting

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
import com.alttester.altTesterExceptions.WaitTimeOutException;

/**
 * Wait until there are no longer any objects that respect the given criteria or
 * times run out and will throw an error.
 */
public class AltWaitForComponentProperty<T> extends AltBaseFindObject {
    /**
     * @param altWaitForComponentPropertyParams the properties parameter for waiting
     *                                          the
     *                                          object
     * @param altObject                         the AltObject element
     * @param property                          the wanted value of the property
     */
    private AltObject altObject;
    private AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams;
    private T property;

    /**
     * @param messageHandler
     * @param altWaitForComponentPropertyParams
     */
    public AltWaitForComponentProperty(IMessageHandler messageHandler,
            AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams, T property, AltObject altObject) {
        super(messageHandler);
        this.altWaitForComponentPropertyParams = altWaitForComponentPropertyParams;
        this.property = property;
        this.altObject = altObject;
    }

    public T Execute(Class<T> returnType) {
        T propertyFound;
        double time = 0;
        while (time < altWaitForComponentPropertyParams.getTimeout()) {
            logger.debug("Waiting for element where name contains "
                    + altWaitForComponentPropertyParams.getAltGetComponentPropertyParams().getPropertyName() + "....");
            try {
                propertyFound = altObject.getComponentProperty(
                        altWaitForComponentPropertyParams.getAltGetComponentPropertyParams(),
                        returnType);

                if (propertyFound.equals(property))
                    return propertyFound;

            } catch (Exception e) {
                logger.warn("Exception thrown: " + e.getLocalizedMessage());
            }
            Utils.sleepFor(altWaitForComponentPropertyParams.getInterval());
            time += altWaitForComponentPropertyParams.getInterval();
        }
        throw new WaitTimeOutException(
                "Property " + altWaitForComponentPropertyParams.getAltGetComponentPropertyParams().getPropertyName()
                        + " still not found after " + altWaitForComponentPropertyParams.getTimeout() + " seconds");
    }
}
