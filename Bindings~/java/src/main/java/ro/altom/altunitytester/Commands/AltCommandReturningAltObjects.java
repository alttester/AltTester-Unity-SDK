package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityObject;

public class AltCommandReturningAltObjects extends AltBaseCommand {

    public AltCommandReturningAltObjects(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    protected AltUnityObject ReceiveAltUnityObject(AltMessage altMessage) {
        AltUnityObject altUnityObject = recvall(altMessage, AltUnityObject.class);
        altUnityObject.setMesssageHandler(messageHandler);
        return altUnityObject;
    }

    protected AltUnityObject[] ReceiveListOfAltUnityObjects(AltMessage altMessage) {
        AltUnityObject[] altUnityObjects = recvall(altMessage, AltUnityObject[].class);
        for (AltUnityObject altUnityObject : altUnityObjects) {
            altUnityObject.setMesssageHandler(messageHandler);
        }
        return altUnityObjects;
    }
}
