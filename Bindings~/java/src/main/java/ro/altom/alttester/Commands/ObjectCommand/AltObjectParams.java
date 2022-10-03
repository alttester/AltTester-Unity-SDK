package ro.altom.alttester.Commands.ObjectCommand;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.AltObject;

public class AltObjectParams extends AltMessage {

    protected AltObject altObject;

    AltObjectParams() {
    }

    public void setAltObject(AltObject altObject) {
        this.altObject = altObject;
    }

}
