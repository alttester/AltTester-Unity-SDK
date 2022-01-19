package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityObject;

public class AltUnityObjectParams extends AltMessage {

    protected AltUnityObject altUnityObject;

    AltUnityObjectParams() {
    }

    public void setAltUnityObject(AltUnityObject altUnityObject) {
        this.altUnityObject = altUnityObject;
    }

}
