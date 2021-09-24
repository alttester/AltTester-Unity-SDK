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
    private AltPressKeyParameters altPressKeyParameters;

    public AltPressKey(IMessageHandler messageHandler, AltPressKeyParameters altPressKeyParameters) {
        super(messageHandler);
        this.altPressKeyParameters = altPressKeyParameters;
        this.altPressKeyParameters.setCommandName("pressKeyboardKey");
    }

    public void Execute() {
        SendCommand(altPressKeyParameters);
        recvall(altPressKeyParameters, String.class);
    }
}
