package com.alttester.Commands.InputActions;

import com.alttester.AltMessage;
import com.alttester.UnityStruct.AltKeyCode;

public class AltKeysUpParams extends AltMessage {

    private AltKeyCode[] keyCodes;

    public static class Builder {
        private AltKeyCode[] keyCodes;

        public Builder(AltKeyCode[] keyCodes) {
            this.keyCodes = keyCodes;
        }

        public AltKeysUpParams build() {
            AltKeysUpParams params = new AltKeysUpParams();
            params.keyCodes = this.keyCodes;
            return params;
        }
    }

    private AltKeysUpParams() {
        this.setCommandName("keysUp");
    }

    public AltKeyCode[] getKeyCode() {
        return keyCodes;
    }

    public void setKeyCode(AltKeyCode[] keyCodes) {
        this.keyCodes = keyCodes;
    }
}
