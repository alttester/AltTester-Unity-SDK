package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

import java.util.List;

/**
 * Similar command like swipe but instead of swipe from point A to point B you
 * are able to give list a points.
 */
public class AltMultiPointSwipe extends AltBaseCommand {

    private AltMultiPointSwipeParameters params;

    public AltMultiPointSwipe(IMessageHandler messageHandler, List<Vector2> positions, float durationInSeconds) {
        super(messageHandler);
        params = new AltMultiPointSwipeParameters(positions, durationInSeconds);
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}