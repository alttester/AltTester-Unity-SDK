package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

/**
 * Simulate mouse movement in your game. This command does not wait for the
 * movement to finish.
 */
public class AltMoveMouse extends AltBaseCommand {

    /**
     * @param params the builder for the mouse moves command.
     */
    private AltMoveMouseParams params;

    public AltMoveMouse(IMessageHandler messageHandler, AltMoveMouseParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);

        if (params.getWait()) {
            data = recvall(params, String.class);
            validateResponse("Finished", data);
        }
    }
}
