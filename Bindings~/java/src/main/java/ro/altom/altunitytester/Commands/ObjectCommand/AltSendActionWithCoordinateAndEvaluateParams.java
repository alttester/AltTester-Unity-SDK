package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltUnityObject;

public class AltSendActionWithCoordinateAndEvaluateParams extends AltUnityObjectParams {

    private AltUnityObject altUnityObject;
    private int x;
    private int y;

    AltSendActionWithCoordinateAndEvaluateParams(AltUnityObject altUnityObject, int x, int y) {
        this.setAltUnityObject(altUnityObject);
        this.setX(x);
        this.setY(y);
    }

    public int getY() {
        return y;
    }

    public void setY(int y) {
        this.y = y;
    }

    public int getX() {
        return x;
    }

    public void setX(int x) {
        this.x = x;
    }

    public AltUnityObject getAltUnityObject() {
        return altUnityObject;
    }

    public void setAltUnityObject(AltUnityObject altUnityObject) {
        this.altUnityObject = altUnityObject;
    }
}
