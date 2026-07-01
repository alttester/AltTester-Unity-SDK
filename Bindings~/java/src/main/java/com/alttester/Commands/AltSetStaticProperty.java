/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands;

import com.alttester.Commands.ObjectCommand.AltSetComponentPropertyParams;
import com.alttester.IMessageHandler;

/** Set the value of a property from one of the component of the object. */
public class AltSetStaticProperty extends AltBaseCommand {
  /**
   * @param altSetComponentPropertyParameters builder for setting components'
   *                                          property
   */
  private AltSetComponentPropertyParams altSetComponentPropertyParameters;

  public AltSetStaticProperty(
      IMessageHandler messageHandler,
      AltSetComponentPropertyParams altSetComponentPropertyParameters) {
    super(messageHandler);
    this.altSetComponentPropertyParameters = altSetComponentPropertyParameters;
    altSetComponentPropertyParameters.setAltObject(null);
    this.altSetComponentPropertyParameters.setCommandName("setObjectComponentProperty");
  }

  public void Execute() {
    SendCommand(altSetComponentPropertyParameters);
    String response = recvall(altSetComponentPropertyParameters, String.class);
    validateResponse("valueSet", response);
  }
}
