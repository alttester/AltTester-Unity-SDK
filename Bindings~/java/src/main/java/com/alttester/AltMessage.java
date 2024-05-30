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

package com.alttester;

import javax.websocket.Encoder;
import javax.websocket.EndpointConfig;
import javax.websocket.DecodeException;
import javax.websocket.Decoder;
import javax.websocket.EncodeException;

import com.google.gson.Gson;

public class AltMessage {
    private String messageId;
    private String commandName;

    public AltMessage() {
        this.messageId = null;
    }

    public String getCommandName() {
        return commandName;
    }

    public void setCommandName(String commandName) {
        this.commandName = commandName;
    }

    public String messageId() {
        return this.messageId;
    }

    public void setMessageId(String messageId) {
        this.messageId = messageId;
    }

    public static class AltMessageEncoder implements Encoder.Text<AltMessage> {
        @Override
        public void init(final EndpointConfig config) {
        }

        @Override
        public String encode(final AltMessage altMessage) throws EncodeException {
            return new Gson().toJson(altMessage);
        }

        @Override
        public void destroy() {
        }
    }

    public static class AltMessageDecoder implements Decoder.Text<AltMessage> {
        @Override
        public void init(final EndpointConfig config) {
        }

        @Override
        public AltMessage decode(final String str) throws DecodeException {
            return new Gson().fromJson(str, AltMessage.class);
        }

        @Override
        public boolean willDecode(final String str) {
            return true;
        }

        @Override
        public void destroy() {
        }
    }
}
