/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.InputActions;

import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltEndTouch extends AltBaseCommand {
  private AltEndTouchParams params;

  public AltEndTouch(IMessageHandler messageHandler, AltEndTouchParams params) {
    super(messageHandler);
    this.params = params;
  }

  public void Execute() {
    SendCommand(params);
    String data = recvall(params, String.class);
    validateResponse("Ok", data);
  }
}
