/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.InputActions;

import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltKeyUp extends AltBaseCommand {

  private AltKeyUpParams params;

  public AltKeyUp(IMessageHandler messageHandler, AltKeyUpParams params) {
    super(messageHandler);
    this.params = params;
  }

  public void Execute() {
    SendCommand(params);
    recvall(params, String.class);
  }
}
