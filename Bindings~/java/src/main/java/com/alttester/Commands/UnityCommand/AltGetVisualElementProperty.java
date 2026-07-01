/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.UnityCommand;

import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltGetVisualElementProperty extends AltBaseCommand {

  private AltGetVisualElementProperyParams params;

  public AltGetVisualElementProperty(
      IMessageHandler messageHandler,
      AltGetVisualElementProperyParams altGetVisualElementProperyParams) {
    super(messageHandler);
    this.params = altGetVisualElementProperyParams;
    this.params.setCommandName("getVisualElementProperty");
  }

  public <T> T Execute(Class<T> returnType) {

    SendCommand(params);
    return recvall(params, returnType);
  }
}
