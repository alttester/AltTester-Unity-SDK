package com.alttester.Commands.InputActions;

import com.alttester.AltMessage;
import com.alttester.UnityStruct.AltKeyCode;

public class AltKeyUpParams extends AltMessage {

    private AltKeyCode keyCode;

    public static class Builder {
        private AltKeyCode keyCode = AltKeyCode.NoKey;

        public Builder(AltKeyCode keyCode) {
            this.keyCode = keyCode;
        }

        public AltKeyUpParams build() {
            AltKeyUpParams params = new AltKeyUpParams();
            params.keyCode = this.keyCode;
            return params;
        }
    }

    private AltKeyUpParams() {
        this.setCommandName("keyUp");
    }

    public AltKeyCode getKeyCode() {
        return keyCode;
    }

    public void setKeyCode(AltKeyCode keyCode) {
        this.keyCode = keyCode;
    }
}
