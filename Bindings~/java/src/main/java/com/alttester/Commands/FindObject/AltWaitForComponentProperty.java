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
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.altTesterExceptions.WaitTimeOutException;

/**
 * Wait until there are no longer any objects that respect the given criteria or
 * times run out and will throw an error.
 */
public class AltWaitForComponentProperty<T> extends AltBaseFindObject {
    /**
     * @param waitParams the properties parameter for waiting
     *                   the
     *                   object
     * @param altObject  the AltObject element
     * @param property   the wanted value of the property
     */
    private AltObject altObject;
    private AltWaitForComponentPropertyParams<T> waitParams;
    private T property;

    /**
     * @param messageHandler
     * @param altWaitForComponentPropertyParams
     */
    public AltWaitForComponentProperty(IMessageHandler messageHandler,
            AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams, T property, AltObject altObject) {
        super(messageHandler);
        this.waitParams = altWaitForComponentPropertyParams;
        this.property = property;
        this.altObject = altObject;
    }

    public T Execute(Class<T> returnType) {
        T propertyFound;
        double time = 0;
        AltGetComponentPropertyParams getComponentPropertyParams = waitParams.getAltGetComponentPropertyParams();
        while (time < waitParams.getTimeout()) {
            logger.debug("Waiting for element where name contains "
                    + getComponentPropertyParams.getPropertyName() + "....");
                propertyFound = altObject.getComponentProperty(
                        getComponentPropertyParams,
                        returnType);

                if (propertyFound.equals(property))
                    return propertyFound;

            Utils.sleepFor(waitParams.getInterval());
            time += waitParams.getInterval();
        }
        throw new WaitTimeOutException(
                "Property " + getComponentPropertyParams.getPropertyName()
                        + " still not found after " + waitParams.getTimeout() + " seconds");
    }
}
