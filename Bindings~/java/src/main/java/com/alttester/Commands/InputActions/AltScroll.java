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

/**
 * Simulate scroll mouse action in your application.
 */
public class AltScroll extends AltBaseCommand {
    private AltScrollParams params;

    public AltScroll(IMessageHandler messageHandler, AltScrollParams params) {
        super(messageHandler);
        this.params = params;
        this.params.setCommandName("scroll");
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);

        if (params.getWait()) {
            data = recvall(params, String.class);
            validateResponse("Finished", data);
        }

    }
}
