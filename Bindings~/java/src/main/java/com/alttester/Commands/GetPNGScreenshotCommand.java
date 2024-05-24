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

package com.alttester.Commands;

import com.alttester.AltMessage;
import com.alttester.IMessageHandler;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Base64;

/**
 * Create a screenshot of the current scene in png format.
 */
public class GetPNGScreenshotCommand extends AltBaseCommand {
    /**
     * @param path location where the image is created
     */
    private String path;

    public GetPNGScreenshotCommand(IMessageHandler messageHandler, String path) {
        super(messageHandler);
        this.path = path;
    }

    public void Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getPNGScreenshot");
        SendCommand(altMessage);
        String data = recvall(altMessage, String.class);
        validateResponse("Ok", data);

        String screenshotData = recvall(altMessage, String.class);
        byte[] screenshotDataBytes = Base64.getDecoder().decode(screenshotData);
        try (FileOutputStream stream = new FileOutputStream(path)) {
            stream.write(screenshotDataBytes);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
