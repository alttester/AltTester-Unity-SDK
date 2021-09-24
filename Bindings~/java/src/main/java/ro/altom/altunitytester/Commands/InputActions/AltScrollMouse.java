package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltScrollMouse extends AltBaseCommand {

    /**
     * @param altScrollMouseParameters the builder for the scroll commands.
     */
    private AltScrollMouseParameters altScrollMouseParameters;

    public AltScrollMouse(IMessageHandler messageHandler, AltScrollMouseParameters altScrollMouseParameters) {
        super(messageHandler);
        this.altScrollMouseParameters = altScrollMouseParameters;
        this.altScrollMouseParameters.setCommandName("scrollMouse");
    }

    public void Execute() {
        SendCommand(altScrollMouseParameters);
        recvall(altScrollMouseParameters, String.class);

    }
}
