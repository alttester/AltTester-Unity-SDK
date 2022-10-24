package com.alttester.Commands.ObjectCommand;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.Commands.AltCommandReturningAltObjects;

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