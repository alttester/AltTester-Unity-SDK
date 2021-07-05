package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeyUp extends AltBaseCommand {

    private AltUnityKeyCode keyCode;

    public AltKeyUp(AltBaseSettings altBaseSettings, AltUnityKeyCode keyCode) {
        super(altBaseSettings);
        this.keyCode = keyCode;
    }

    public void Execute() {
        SendCommand("keyUp", keyCode.toString());
        recvall();
    }
}
