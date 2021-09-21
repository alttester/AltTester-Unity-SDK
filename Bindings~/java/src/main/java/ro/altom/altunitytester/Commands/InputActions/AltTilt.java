package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulates device rotation action in your game.
 */
public class AltTilt extends AltBaseCommand {
    private AltTiltParameters altTiltParameters;

    public AltTilt(IMessageHandler messageHandler, AltTiltParameters altTiltParameters) {
        super(messageHandler);
        this.altTiltParameters = altTiltParameters;
    }

    public void Execute() {
        SendCommand(altTiltParameters);
        String data = recvall(altTiltParameters, String.class);
        validateResponse("Ok", data);
    }
}
