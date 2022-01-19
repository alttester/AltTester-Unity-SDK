package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltSetText extends AltCommandReturningAltObjects {

    private AltSetTextParams params;

    public AltSetText(IMessageHandler messageHandler, AltSetTextParams params) {
        super(messageHandler);
        this.params = params;
        params.setCommandName("setText");
    }

    public AltUnityObject Execute() {
        SendCommand(params);
        return ReceiveAltUnityObject(params);
    }
}