/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.UnityCommand;

import com.alttester.AltDriver;
import com.alttester.Commands.AltBaseCommand;
import com.alttester.IMessageHandler;

/** Delete from games player pref a key */
public class AltFloatGetKeyPlayerPref extends AltBaseCommand {

  private AltKeyPlayerPrefParams params;

  public AltFloatGetKeyPlayerPref(IMessageHandler messageHandler, String keyName) {
    super(messageHandler);
    params = new AltKeyPlayerPrefParams(keyName, AltDriver.PlayerPrefsKeyType.Float);
    params.setCommandName("getKeyPlayerPref");
  }

  public float Execute() {
    SendCommand(params);
    return recvall(params, Float.class);
  }
}
