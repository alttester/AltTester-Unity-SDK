package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

/**
 * Simulates device rotation action in your game.
 */
public class AltTilt extends AltBaseCommand {
    private AltTiltParams altTiltParameters;

    public AltTilt(IMessageHandler messageHandler, AltTiltParams altTiltParameters) {
        super(messageHandler);
        this.altTiltParameters = altTiltParameters;
    }

    public void Execute() {
        SendCommand(altTiltParameters);
        String data = recvall(altTiltParameters, String.class);
        validateResponse("Ok", data);

        if (altTiltParameters.getWait()) {
            data = recvall(altTiltParameters, String.class);
            validateResponse("Finished", data);
        }
    }
}
