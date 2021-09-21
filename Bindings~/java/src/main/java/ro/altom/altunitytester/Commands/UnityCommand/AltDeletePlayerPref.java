package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

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
