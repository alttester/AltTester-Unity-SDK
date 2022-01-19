package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Similar command like swipe but instead of swipe from point A to point B you
 * are able to give list a points.
 */
public class AltPressKey extends AltBaseCommand {
    /**
     * @param altPressKeyParameters the builder for the press key commands.
     */
    private AltPressKeyParams altPressKeyParameters;

    public AltPressKey(IMessageHandler messageHandler, AltPressKeyParams altPressKeyParameters) {
        super(messageHandler);
        this.altPressKeyParameters = altPressKeyParameters;
        this.altPressKeyParameters.setCommandName("pressKeyboardKey");
    }

    public void Execute() {
        SendCommand(altPressKeyParameters);
        String data = recvall(altPressKeyParameters, String.class);
        validateResponse("Ok", data);

        if (altPressKeyParameters.getWait()) {
            data = recvall(altPressKeyParameters, String.class);
            validateResponse("Finished", data);
        }
    }
}
