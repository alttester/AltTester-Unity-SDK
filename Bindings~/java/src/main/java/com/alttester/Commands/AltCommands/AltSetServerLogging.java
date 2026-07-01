/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.AltCommands;

import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltSetServerLogging extends AltBaseCommand {

  private AltSetServerLoggingParams setServerLoggingParameters;

  public AltSetServerLogging(
      IMessageHandler messageHandler, AltSetServerLoggingParams setServerLoggingParameters) {
    super(messageHandler);

    this.setServerLoggingParameters = setServerLoggingParameters;
  }

  public void Execute() {
    SendCommand(setServerLoggingParameters);
    recvall(setServerLoggingParameters, String.class);
  }
}
