package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate mouse movement in your game. This command does not wait for the
 * movement to finish.
 */
public class AltMoveMouseAndWait extends AltBaseCommand {

    /**
     * @param altMoveMouseParameters the builder for the mouse moves command.
     */
    private AltMoveMouseParameters altMoveMouseParameters;

    public AltMoveMouseAndWait(IMessageHandler messageHandler, AltMoveMouseParameters altMoveMouseParameters) {
        super(messageHandler);
        this.altMoveMouseParameters = altMoveMouseParameters;
    }

    public void Execute() {
        new AltMoveMouse(messageHandler, altMoveMouseParameters).Execute();
        sleepFor(altMoveMouseParameters.getDuration());
        String data;
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("actionFinished");
        do {
            SendCommand(altMessage);
            data = recvall(altMoveMouseParameters, String.class);
        } while (data.equals("No"));

        validateResponse("Yes", data);
    }
}
