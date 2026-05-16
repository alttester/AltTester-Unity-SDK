/*
    Copyright(C) 2026 Altom Consulting

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
import com.alttester.altTesterExceptions.*;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class AltBaseCommand {
  protected static final Logger logger = LoggerFactory.getLogger(AltBaseCommand.class);

  protected IMessageHandler messageHandler;

  public AltBaseCommand(IMessageHandler messageHandler) {
    this.messageHandler = messageHandler;
  }

  protected <T> T recvall(AltMessage altMessage, final Class<T> type) {

    return messageHandler.receive(altMessage, type);
  }

  protected synchronized void SendCommand(AltMessage altMessage) {
    altMessage.setMessageId(Long.toString(System.nanoTime()));
    messageHandler.send(altMessage);
  }

  protected void validateResponse(String expected, String received) {
    if (!expected.equals(received)) {
      throw new AltInvalidServerResponse(expected, received);
    }
  }
}
