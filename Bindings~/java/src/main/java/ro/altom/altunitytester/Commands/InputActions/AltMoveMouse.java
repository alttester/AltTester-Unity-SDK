package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate mouse movement in your game. This command does not wait for the
 * movement to finish.
 */
public class AltMoveMouse extends AltBaseCommand {

    /**
     * @param altMoveMouseParameters the builder for the mouse moves command.
     */
    private AltMoveMouseParameters altMoveMouseParameters;

    public AltMoveMouse(AltBaseSettings altBaseSettings, AltMoveMouseParameters altMoveMouseParameters) {
        super(altBaseSettings);
        this.altMoveMouseParameters = altMoveMouseParameters;
    }

    public void Execute() {
        SendCommand("moveMouse", vectorToJsonString(altMoveMouseParameters.getX(), altMoveMouseParameters.getY()),
                String.valueOf(altMoveMouseParameters.getDuration()));
        recvall();
    }
}
