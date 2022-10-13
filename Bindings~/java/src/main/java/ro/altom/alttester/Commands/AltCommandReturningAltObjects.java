package ro.altom.alttester.Commands;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltMessage;
import ro.altom.alttester.AltObject;

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
