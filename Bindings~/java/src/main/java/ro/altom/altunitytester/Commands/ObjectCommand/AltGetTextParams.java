package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityObject;

public class AltGetTextParams extends AltMessage {
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;

    public AltGetTextParams(AltUnityObject altUnityObject) {
        this.altUnityObject = altUnityObject;
    }

    public AltUnityObject getAltUnityObject() {
        return altUnityObject;
    }

    public void setAltUnityObject(AltUnityObject altUnityObject) {
        this.altUnityObject = altUnityObject;
    }
}
