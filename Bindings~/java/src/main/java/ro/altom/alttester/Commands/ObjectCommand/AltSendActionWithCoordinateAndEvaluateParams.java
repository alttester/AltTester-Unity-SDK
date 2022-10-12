package ro.altom.alttester.Commands.ObjectCommand;

import ro.altom.alttester.AltObject;

public class AltSendActionWithCoordinateAndEvaluateParams extends AltObjectParams {

    private AltObject altObject;
    private int x;
    private int y;

    AltSendActionWithCoordinateAndEvaluateParams(AltObject altObject, int x, int y) {
        this.setAltObject(altObject);
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

    public AltObject getAltObject() {
        return altObject;
    }

    public void setAltObject(AltObject altObject) {
        this.altObject = altObject;
    }
}
