package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityObject;

public class AltGetTextParameters extends AltMessage{
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;

    public AltGetTextParameters(AltUnityObject altUnityObject){
        this.altUnityObject = altUnityObject;
    }

    public AltUnityObject getAltUnityObject() {
        return altUnityObject;
    }

    public void setAltUnityObject(AltUnityObject altUnityObject) {
        this.altUnityObject = altUnityObject;
    }
}
