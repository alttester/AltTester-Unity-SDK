/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltGetAllLoadedScenes extends AltBaseCommand {
  public AltGetAllLoadedScenes(IMessageHandler messageHandler) {
    super(messageHandler);
  }

  public String[] Execute() {
    AltMessage altMessage = new AltMessage();
    altMessage.setCommandName("getAllLoadedScenes");
    SendCommand(altMessage);
    return recvall(altMessage, String[].class);
  }
}
