package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltSwipe extends AltBaseCommand {
   
    private AltSwipeParameters params;

    public AltSwipe(IMessageHandler messageHandler, int xStart, int yStart, int xEnd, int yEnd,
            float durationInSeconds) {
        super(messageHandler);
        params = new AltSwipeParameters(xStart, yStart, xEnd, yEnd, durationInSeconds);
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
