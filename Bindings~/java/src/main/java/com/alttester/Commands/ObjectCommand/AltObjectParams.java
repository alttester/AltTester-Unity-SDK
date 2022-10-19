package com.alttester.Commands.ObjectCommand;

import com.alttester.AltMessage;
import com.alttester.AltObject;

public class AltObjectParams extends AltMessage {

    protected AltObject altObject;

    AltObjectParams() {
    }

    public void setAltObject(AltObject altObject) {
        this.altObject = altObject;
    }

}
