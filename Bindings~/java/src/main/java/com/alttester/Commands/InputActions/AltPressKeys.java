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
 * Similar command like swipe but instead of swipe from point A to point B you
 * are able to give list a points.
 */
public class AltPressKeys extends AltBaseCommand {
    /**
     * @param altPressKeysParameters the builder for the press key commands.
     */
    private AltPressKeysParams altPressKeysParameters;

    public AltPressKeys(IMessageHandler messageHandler, AltPressKeysParams altPressKeysParameters) {
        super(messageHandler);
        this.altPressKeysParameters = altPressKeysParameters;
        this.altPressKeysParameters.setCommandName("pressKeyboardKeys");
    }

    public void Execute() {
        SendCommand(altPressKeysParameters);
        String data = recvall(altPressKeysParameters, String.class);
        validateResponse("Ok", data);

        if (altPressKeysParameters.getWait()) {
            for (int i = 0; i < altPressKeysParameters.getKeyCodes().length; i++) {
                data = recvall(altPressKeysParameters, String.class);
                validateResponse("Finished", data);
            }
        }
    }
}
