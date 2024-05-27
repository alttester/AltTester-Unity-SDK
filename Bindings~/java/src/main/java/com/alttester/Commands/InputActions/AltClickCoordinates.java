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

package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltClickCoordinates extends AltBaseCommand {
    private AltTapClickCoordinatesParams parameters;

    public AltClickCoordinates(IMessageHandler messageHandler, AltTapClickCoordinatesParams parameters) {
        super(messageHandler);
        this.parameters = parameters;
        this.parameters.setCommandName("clickCoordinates");
    }

    public void Execute() {
        SendCommand(parameters);
        String data = recvall(parameters, String.class);
        validateResponse("Ok", data);
        if (parameters.getWait()) {
            data = recvall(parameters, String.class);
            validateResponse("Finished", data);
        }
    }
}
