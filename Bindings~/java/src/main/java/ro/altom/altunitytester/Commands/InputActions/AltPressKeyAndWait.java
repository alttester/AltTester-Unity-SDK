package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Similar command like swipe but instead of swipe from point A to point B you
 * are able to give list a points.
 */
public class AltPressKeyAndWait extends AltBaseCommand {

    /**
     * @param altPressKeyParameters the builder for the press key commands.
     */
    private AltPressKeyParameters altPressKeyParameters;

    public AltPressKeyAndWait(IMessageHandler messageHandler, AltPressKeyParameters altPressKeyParameters) {
        super(messageHandler);
        this.altPressKeyParameters = altPressKeyParameters;
    }

    public void Execute() {
        new AltPressKey(messageHandler, altPressKeyParameters).Execute();
        sleepFor(altPressKeyParameters.getDuration());
        String data;
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("actionFinished");
        do {
            SendCommand(altMessage);
            data = recvall(altPressKeyParameters, String.class);
        } while (data.equals("No"));
        validateResponse("Yes", data);
    }
}
