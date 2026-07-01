/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.UnityCommand;

import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

public class AltUnloadScene extends AltBaseCommand {

  private AltUnloadSceneParams params;

  public AltUnloadScene(IMessageHandler messageHandler, AltUnloadSceneParams params) {
    super(messageHandler);
    this.params = params;
    params.setCommandName("unloadScene");
  }

  public void Execute() {
    SendCommand(params);
    String data = recvall(params, String.class);
    validateResponse("Ok", data);

    data = recvall(params, String.class);
    validateResponse("Scene Unloaded", data);
  }
}
