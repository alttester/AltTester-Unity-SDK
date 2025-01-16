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

package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltBeginTouch extends AltBaseCommand {
    private AltBeginTouchParams params;

    public AltBeginTouch(IMessageHandler messageHandler, AltBeginTouchParams params) {
        super(messageHandler);
        this.params = params;
    }

    public int Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        return Integer.parseInt(data);
    }
}
