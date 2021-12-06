package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
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

    public AltMoveMouse(IMessageHandler messageHandler, AltMoveMouseParameters altMoveMouseParameters) {
        super(messageHandler);
        this.altMoveMouseParameters = altMoveMouseParameters;
    }

    public void Execute() {
        SendCommand(altMoveMouseParameters);
        String data = recvall(altMoveMouseParameters, String.class);
        validateResponse("Ok", data);

        if (altMoveMouseParameters.getWait()) {
            data = recvall(altMoveMouseParameters, String.class);
            validateResponse("Finished", data);
        }
    }
}
