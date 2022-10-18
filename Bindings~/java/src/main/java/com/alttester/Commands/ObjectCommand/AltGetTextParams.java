package com.alttester.Commands.ObjectCommand;

import com.alttester.AltMessage;
import com.alttester.AltObject;

public class AltGetTextParams extends AltMessage {
    /**
     * @param altObject The game object
     */
    private AltObject altObject;

    public AltGetTextParams(AltObject altObject) {
        this.altObject = altObject;
    }

    public AltObject getAltObject() {
        return altObject;
    }

    public void setAltObject(AltObject altObject) {
        this.altObject = altObject;
    }
}
