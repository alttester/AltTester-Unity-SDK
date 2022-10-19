package com.alttester.Commands.ObjectCommand;

import com.alttester.AltObject;

public class AltSendActionAndEvaluateResultParams extends AltObjectParams {

    AltSendActionAndEvaluateResultParams(AltObject altObject) {
        super.altObject = altObject;
    }

    public AltObject getAltObject() {
        return altObject;
    }

    public void setAltObject(AltObject altObject) {
        this.altObject = altObject;
    }
}
