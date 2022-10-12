package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltSwipe extends AltBaseCommand {

    private AltSwipeParams params;

    public AltSwipe(IMessageHandler messageHandler, AltSwipeParams params) {
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
