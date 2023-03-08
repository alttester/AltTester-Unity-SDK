package com.alttester.Commands;

import com.alttester.AltMessage;
import com.alttester.AltObject;
import com.alttester.IMessageHandler;

public class AltCommandReturningAltObjects extends AltBaseCommand {

    public AltCommandReturningAltObjects(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    protected AltObject ReceiveAltObject(AltMessage altMessage) {
        AltObject altObject = recvall(altMessage, AltObject.class);
        if (altObject != null)
            altObject.setMesssageHandler(messageHandler);
        return altObject;
    }

    protected AltObject[] ReceiveListOfAltObjects(AltMessage altMessage) {
        AltObject[] altObjects = recvall(altMessage, AltObject[].class);
        for (AltObject altObject : altObjects) {
            altObject.setMesssageHandler(messageHandler);
        }
        return altObjects;
    }
}
