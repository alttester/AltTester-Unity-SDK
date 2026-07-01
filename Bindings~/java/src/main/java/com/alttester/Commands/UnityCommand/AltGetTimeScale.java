/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltGetTimeScale extends AltBaseCommand {

  public AltGetTimeScale(IMessageHandler messageHandler) {
    super(messageHandler);
  }

  public float Execute() {
    AltMessage altMessage = new AltMessage();
    altMessage.setCommandName("getTimeScale");
    SendCommand(altMessage);
    return recvall(altMessage, Float.class);
  }
}
