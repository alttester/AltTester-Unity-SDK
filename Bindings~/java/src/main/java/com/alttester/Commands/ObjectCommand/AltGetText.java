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
 * Get text value from a Button, Text, InputField. This also works with
 * TextMeshPro elements.
 */
public class AltGetText extends AltBaseCommand {

    private AltGetTextParams params;

    public AltGetText(IMessageHandler messageHandler, AltGetTextParams params) {
        super(messageHandler);
        this.params = params;
        params.setCommandName("getText");
        ;
    }

    public String Execute() {
        SendCommand(params);
        return recvall(params, String.class);
    }
}
