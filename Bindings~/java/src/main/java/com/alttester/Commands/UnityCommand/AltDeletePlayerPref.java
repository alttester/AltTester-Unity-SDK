package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

/**
 * Delete entire player pref of the game
 */
public class AltDeletePlayerPref extends AltBaseCommand {
    public AltDeletePlayerPref(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public void Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("deletePlayerPref");
        SendCommand(altMessage);
        String data = recvall(altMessage, String.class);
        validateResponse("Ok", data);
    }
}
