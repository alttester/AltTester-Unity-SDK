package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltFindObjectAtCoordinates extends AltCommandReturningAltObjects {
    private AltFindObjectAtCoordinatesParams params;

    public AltFindObjectAtCoordinates(IMessageHandler messageHandler, AltFindObjectAtCoordinatesParams params) {
        super(messageHandler);
        this.params = params;
        this.params.setCommandName("findObjectAtCoordinates");
    }

    public AltUnityObject Execute() {
        SendCommand(params);
        return ReceiveAltUnityObject(params);
    }
}
