package ro.altom.altunitytester.Commands;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

public class AltCommandReturningAltObjects extends AltBaseCommand {

    public AltCommandReturningAltObjects(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }

    protected AltUnityObject ReceiveAltUnityObject() {
        String data = recvall();

        AltUnityObject altUnityObject = new Gson().fromJson(data, AltUnityObject.class);
        altUnityObject.setAltBaseSettings(altBaseSettings);
        return altUnityObject;
    }

    protected AltUnityObject[] ReceiveListOfAltUnityObjects() {
        String data = recvall();
        AltUnityObject[] altUnityObjects = new Gson().fromJson(data, AltUnityObject[].class);
        for (AltUnityObject altUnityObject : altUnityObjects) {
            altUnityObject.setAltBaseSettings(altBaseSettings);
        }
        return altUnityObjects;
    }
}
