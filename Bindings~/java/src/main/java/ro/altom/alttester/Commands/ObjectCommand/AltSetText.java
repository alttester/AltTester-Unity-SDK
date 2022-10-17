package ro.altom.alttester.Commands.ObjectCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltObject;
import ro.altom.alttester.Commands.AltCommandReturningAltObjects;

public class AltSetText extends AltCommandReturningAltObjects {

    private AltSetTextParams params;

    public AltSetText(IMessageHandler messageHandler, AltSetTextParams params) {
        super(messageHandler);
        this.params = params;
        params.setCommandName("setText");
    }

    public AltObject Execute() {
        SendCommand(params);
        return ReceiveAltObject(params);
    }
}