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

package com.alttester.Commands.ObjectCommand;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

/**
 * Invoke a method from an existing component of the object.
 */
public class AltCallComponentMethod extends AltBaseCommand {

    /**
     * @param altCallComponentMethodParameters builder for calling component methods
     */
    private AltCallComponentMethodParams altCallComponentMethodParameters;

    public AltCallComponentMethod(IMessageHandler messageHandler,
            AltCallComponentMethodParams altCallComponentMethodParameters) {
        super(messageHandler);
        this.altCallComponentMethodParameters = altCallComponentMethodParameters;
        this.altCallComponentMethodParameters.setCommandName("callComponentMethodForObject");
    }

    public <T> T Execute(Class<T> returnType) {
        SendCommand(altCallComponentMethodParameters);
        return recvall(altCallComponentMethodParameters, returnType);
    }
}
