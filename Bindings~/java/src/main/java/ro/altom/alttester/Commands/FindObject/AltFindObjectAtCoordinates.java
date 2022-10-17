package ro.altom.alttester.Commands.FindObject;

import ro.altom.alttester.AltObject;
import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltCommandReturningAltObjects;

public class AltFindObjectAtCoordinates extends AltCommandReturningAltObjects {
    private AltFindObjectAtCoordinatesParams params;

    public AltFindObjectAtCoordinates(IMessageHandler messageHandler, AltFindObjectAtCoordinatesParams params) {
        super(messageHandler);
        this.params = params;
        this.params.setCommandName("findObjectAtCoordinates");
    }

    public AltObject Execute() {
        SendCommand(params);
        return ReceiveAltObject(params);
    }
}
