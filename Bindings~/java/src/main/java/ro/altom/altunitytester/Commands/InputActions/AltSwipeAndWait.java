package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltSwipeAndWait extends AltBaseCommand {
    
    private AltSwipeParameters params;

    public AltSwipeAndWait(IMessageHandler messageHandler, int xStart, int yStart, int xEnd, int yEnd,
            float durationInSeconds) {
        super(messageHandler);
        params = new AltSwipeParameters(xStart, yStart, xEnd, yEnd, durationInSeconds);
    }

    public void Execute() {
        new AltSwipe(messageHandler, params.getxStart(), params.getyStart(), params.getxEnd(), params.getyEnd(), params.getDurationInSeconds()).Execute();
        sleepFor(params.getDurationInSeconds());
        String data;
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("actionFinished");
        do {
            SendCommand(altMessage);
            data = recvall(params, String.class);
        } while (data.equals("No"));
        validateResponse("Yes", data);
    }
}
