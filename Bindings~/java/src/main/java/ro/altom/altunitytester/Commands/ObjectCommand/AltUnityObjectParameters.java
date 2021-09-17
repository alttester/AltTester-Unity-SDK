package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityObject;

public class AltUnityObjectParameters extends AltMessage{

    protected AltUnityObject altUnityObject;

    AltUnityObjectParameters(){
    }

    public void setAltUnityObject(AltUnityObject altUnityObject) {
        this.altUnityObject = altUnityObject;
    }
    
}
