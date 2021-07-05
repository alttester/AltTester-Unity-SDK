package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltKeyDown extends AltBaseCommand {

    private AltKeyParameters altKeyDownParameters;

    public AltKeyDown(AltBaseSettings altBaseSettings, AltKeyParameters altKeyDownParameters) {
        super(altBaseSettings);
        this.altKeyDownParameters = altKeyDownParameters;
    }

    public void Execute() {
        String keyCode = altKeyDownParameters.getKeyCode().toString();
        SendCommand("keyDown", keyCode, String.valueOf(altKeyDownParameters.getPower()));
        recvall();
    }
}
