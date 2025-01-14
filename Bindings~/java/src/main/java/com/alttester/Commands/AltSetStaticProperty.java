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

package com.alttester.Commands;

import com.alttester.IMessageHandler;
import com.alttester.Commands.ObjectCommand.AltSetComponentPropertyParams;

/**
 * Set the value of a property from one of the component of the object.
 */
public class AltSetStaticProperty extends AltBaseCommand {
    /**
     * @param altSetComponentPropertyParameters builder for setting components'
     *                                          property
     */
    private AltSetComponentPropertyParams altSetComponentPropertyParameters;

    public AltSetStaticProperty(IMessageHandler messageHandler,
            AltSetComponentPropertyParams altSetComponentPropertyParameters) {
        super(messageHandler);
        this.altSetComponentPropertyParameters = altSetComponentPropertyParameters;
        altSetComponentPropertyParameters.setAltObject(null);
        this.altSetComponentPropertyParameters.setCommandName("setObjectComponentProperty");
    }

    public void Execute() {
        SendCommand(altSetComponentPropertyParameters);
        String response = recvall(altSetComponentPropertyParameters, String.class);
        validateResponse("valueSet", response);
    }

}
