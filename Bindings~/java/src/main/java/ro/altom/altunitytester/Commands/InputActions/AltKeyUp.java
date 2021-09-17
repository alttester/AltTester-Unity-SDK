package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeyUp extends AltBaseCommand {

    private AltKeyUpParameters params;

    public AltKeyUp(IMessageHandler messageHandler, AltUnityKeyCode keyCode) {
        super(messageHandler);
        params = new AltKeyUpParameters(keyCode);
    }

    public void Execute() {
        SendCommand(params);
        recvall(params, String.class);
    }
}
