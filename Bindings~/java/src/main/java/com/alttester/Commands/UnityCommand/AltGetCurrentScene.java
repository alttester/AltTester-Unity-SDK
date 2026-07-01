/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.AltObject;
import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltGetCurrentScene extends AltBaseCommand {
  public AltGetCurrentScene(IMessageHandler messageHandler) {
    super(messageHandler);
  }

  public String Execute() {
    AltMessage altMessage = new AltMessage();
    altMessage.setCommandName("getCurrentScene");
    SendCommand(altMessage);
    AltObject scene = recvall(altMessage, AltObject.class);
    return scene.name;
  }
}
