package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;
import ro.altom.altunitytester.AltMessage;

import java.util.List;

/**
 * Similar command like swipe but instead of swipe from point A to point B you
 * are able to give list a points.
 */
public class AltMultiPointSwipeAndWait extends AltBaseCommand {

    private AltMultiPointSwipeParameters params;

    public AltMultiPointSwipeAndWait(IMessageHandler messageHandler, List<Vector2> positions, float durationInSeconds) {
        super(messageHandler);
        params = new AltMultiPointSwipeParameters(positions, durationInSeconds);
    }

    public void Execute() {
        new AltMultiPointSwipe(messageHandler, params.getPositions(), params.getDurationInSeconds()).Execute();
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