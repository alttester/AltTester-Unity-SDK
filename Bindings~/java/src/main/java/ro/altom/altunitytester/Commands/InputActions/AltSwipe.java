package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltSwipe extends AltBaseCommand {

    private AltSwipeParameters params;

    public AltSwipe(IMessageHandler messageHandler, AltSwipeParameters params) {
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
