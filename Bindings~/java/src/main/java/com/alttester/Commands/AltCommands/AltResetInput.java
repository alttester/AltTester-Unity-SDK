/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.AltCommands;

import com.alttester.AltMessage;
import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltResetInput extends AltBaseCommand {
  public AltResetInput(IMessageHandler messageHandler) {
    super(messageHandler);
  }

  public String Execute() {
    AltMessage altMessage = new AltMessage();
    altMessage.setCommandName("resetInput");
    SendCommand(altMessage);
    return recvall(altMessage, String.class);
  }
}
