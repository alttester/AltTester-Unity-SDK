package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game.
 */
public class AltScroll extends AltBaseCommand {
    private AltScrollParams params;

    public AltScroll(IMessageHandler messageHandler, AltScrollParams params) {
        super(messageHandler);
        this.params = params;
        this.params.setCommandName("scroll");
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
