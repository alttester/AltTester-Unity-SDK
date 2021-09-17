package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltTiltAndWait extends AltBaseCommand {
    private AltTiltParameters altTiltParameters;

    public AltTiltAndWait(IMessageHandler messageHandler, AltTiltParameters altTiltParameters) {
        super(messageHandler);
        this.altTiltParameters = altTiltParameters;
    }

    public void Execute() {
        new AltTilt(messageHandler, altTiltParameters).Execute();
        sleepFor(altTiltParameters.getDuration());
        String data;
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("actionFinished");
        do {
            SendCommand(altMessage);
            data = recvall(altTiltParameters, String.class);
        } while (data.equals("No"));

        validateResponse("Yes", data);
    }
}
