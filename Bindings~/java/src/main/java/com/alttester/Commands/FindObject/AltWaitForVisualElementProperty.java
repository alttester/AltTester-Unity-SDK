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
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;
import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.altTesterExceptions.WaitTimeOutException;
import com.google.gson.Gson;
import com.google.gson.JsonArray;

/**
 * Wait until there are no longer any objects that respect the given criteria or
 * times run out and will throw an error.
 */
public class AltWaitForVisualElementProperty<T> extends AltBaseFindObject {
    /**
     * @param waitParams          the properties parameter for waiting
     *                            the
     *                            object
     * @param altObject           the AltObject element
     * @param property            the wanted value of the property
     * @param getPropertyAsString if true compares the property's value and the
     *                            actual value as strings
     */
    private AltObject altObject;
    private AltWaitForVisualElementPropertyParams<T> waitParams;
    private T property;
    private Boolean getPropertyAsString = false;

    /**
     * @param messageHandler
     * @param AltWaitForVisualElementPropertyParams
     */
    public AltWaitForVisualElementProperty(IMessageHandler messageHandler,
            AltWaitForVisualElementPropertyParams<T> AltWaitForVisualElementPropertyParams, T property,
            AltObject altObject) {
        super(messageHandler);
        this.waitParams = AltWaitForVisualElementPropertyParams;
        this.property = property;
        this.altObject = altObject;
    }

    public AltWaitForVisualElementProperty(IMessageHandler messageHandler,
            AltWaitForVisualElementPropertyParams<T> AltWaitForVisualElementPropertyParams, T property,
            Boolean getPropertyAsString, AltObject altObject) {
        super(messageHandler);
        this.waitParams = AltWaitForVisualElementPropertyParams;
        this.property = property;
        this.getPropertyAsString = getPropertyAsString;
        this.altObject = altObject;
    }

    public T Execute(Class<T> returnType) {
        double time = 0;
        String jsonElementToString = "";
        String propertyName = waitParams.getPropertyName();
        while (time < waitParams.getTimeout()) {
            logger.debug("Waiting for element where name contains "
                    + propertyName + "....");
            T propertyFound = altObject.getVisualElementProperty(
                    propertyName,
                    returnType);
            if (!getPropertyAsString && propertyFound.equals(property))
                return propertyFound;
            if (!(propertyFound instanceof JsonArray)) {
                String str = new Gson().toJsonTree(propertyFound).toString();
                jsonElementToString = str.contains("\"") ? str : "\"" + str + "\"";
            } else {
                jsonElementToString = propertyFound.toString();
            }
            if (getPropertyAsString && jsonElementToString.equals(property.toString()))
                return propertyFound;

            Utils.sleepFor(waitParams.getInterval());
            time += waitParams.getInterval();
        }
        throw new WaitTimeOutException(
                "Property " + propertyName
                        + " was " + jsonElementToString + " and was not " + property + " after "
                        + waitParams.getTimeout()
                        + " seconds");
    }
}
