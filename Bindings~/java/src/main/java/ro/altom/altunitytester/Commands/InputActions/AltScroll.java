package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game.
 */
public class AltScroll extends AltBaseCommand {

    /**
     * @param altScrollParameters the builder for the scroll commands.
     */
    private AltScrollParameters altScrollParameters;

    public AltScroll(IMessageHandler messageHandler, AltScrollParameters altScrollParameters) {
        super(messageHandler);
        this.altScrollParameters = altScrollParameters;
        this.altScrollParameters.setCommandName("scroll");
    }

    public void Execute() {
        SendCommand(altScrollParameters);
        String data = recvall(altScrollParameters, String.class);
        validateResponse("Ok", data);

        if (altScrollParameters.getWait()) {
            data = recvall(altScrollParameters, String.class);
            validateResponse("Finished", data);
        }

    }
}
