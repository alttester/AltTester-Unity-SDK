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
import com.alttester.AltObject;
import com.alttester.Commands.AltBaseCommand;

public class AltSendActionWithCoordinateAndEvaluate extends AltBaseCommand {

    private AltSendActionWithCoordinateAndEvaluateParams params;

    public AltSendActionWithCoordinateAndEvaluate(IMessageHandler messageHandler, AltObject altObject, int x,
            int y, String command) {
        super(messageHandler);
        params = new AltSendActionWithCoordinateAndEvaluateParams(altObject, x, y);
        params.setCommandName(command);
    }

    public AltObject Execute() {
        SendCommand(params);
        return recvall(params, AltObject.class);
    }
}
