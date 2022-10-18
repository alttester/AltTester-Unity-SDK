package com.alttester.Commands.FindObject;

import com.alttester.AltObject;
import com.alttester.IMessageHandler;
import com.alttester.Commands.AltCommandReturningAltObjects;

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
