package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Delete entire player pref of the game
 */
public class AltDeletePlayerPref extends AltBaseCommand {
    public AltDeletePlayerPref(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public void Execute(){
        send(CreateCommand("deletePlayerPref"));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }
}
