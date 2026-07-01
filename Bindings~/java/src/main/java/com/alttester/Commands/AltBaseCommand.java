/*
    Copyright(C) 2026 Altom Consulting
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
