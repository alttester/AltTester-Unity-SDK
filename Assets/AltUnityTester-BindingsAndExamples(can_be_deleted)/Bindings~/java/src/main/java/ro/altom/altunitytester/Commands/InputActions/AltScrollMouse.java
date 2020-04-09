package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the action to finish.
 */
public class AltScrollMouse extends AltBaseCommand {

    /**
     * @param altScrollMouseParameters the builder for the scroll commands.
     */
    private AltScrollMouseParameters altScrollMouseParameters;
    public AltScrollMouse(AltBaseSettings altBaseSettings, AltScrollMouseParameters altScrollMouseParameters) {
        super(altBaseSettings);
        this.altScrollMouseParameters = altScrollMouseParameters;
    }
    public void Execute(){
        send(CreateCommand("scrollMouse", String.valueOf(altScrollMouseParameters.getSpeed()),String.valueOf(altScrollMouseParameters.getDuration())));
        String data = recvall();
        if (!data.contains("error:")) {
            return;
        }
        handleErrors(data);
    }
}
