package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Similar command like swipe but instead of swipe from point A to point B you are able to give list a points. 
 */
public class AltPressKey extends AltBaseCommand {
    /**
     * @param altPressKeyParameters the builder for the press key commands.
     */
    private AltPressKeyParameters altPressKeyParameters;
    public AltPressKey(AltBaseSettings altBaseSettings, AltPressKeyParameters altPressKeyParameters) {
        super(altBaseSettings);
        this.altPressKeyParameters = altPressKeyParameters;
    }
    public void Execute(){
        send(CreateCommand("pressKeyboardKey", altPressKeyParameters.getKeyName(),String.valueOf(altPressKeyParameters.getPower()),String.valueOf(altPressKeyParameters.getDuration())));
        String data = recvall();
        if (!data.contains("error:")) {
            return;
        }
        handleErrors(data);
    }
}
