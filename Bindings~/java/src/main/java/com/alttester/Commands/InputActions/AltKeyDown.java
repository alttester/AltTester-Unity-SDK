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

public class AltKeyDown extends AltBaseCommand {

    private AltKeyDownParams altKeyDownParameters;

    public AltKeyDown(IMessageHandler messageHandler, AltKeyDownParams altKeyDownParameters) {
        super(messageHandler);
        this.altKeyDownParameters = altKeyDownParameters;
    }

    public void Execute() throws InterruptedException {
        SendCommand(altKeyDownParameters);
        recvall(altKeyDownParameters, String.class);
        Thread.sleep(100);
    }
}
