/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.ObjectCommand;

import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltSetComponentProperty extends AltBaseCommand {
  private AltSetComponentPropertyParams altSetComponentPropertyParameters;

  public AltSetComponentProperty(
      IMessageHandler messageHandler,
      AltSetComponentPropertyParams altSetComponentPropertyParameters) {
    super(messageHandler);
    this.altSetComponentPropertyParameters = altSetComponentPropertyParameters;
    this.altSetComponentPropertyParameters.setCommandName("setObjectComponentProperty");
  }

  public void Execute() {
    SendCommand(altSetComponentPropertyParameters);
    String response = recvall(altSetComponentPropertyParameters, String.class);
    validateResponse("valueSet", response);
  }
}
