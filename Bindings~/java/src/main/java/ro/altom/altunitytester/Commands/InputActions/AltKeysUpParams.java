package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeysUpParams extends AltMessage {

    private AltUnityKeyCode[] keyCodes;

    public static class Builder {
        private AltUnityKeyCode[] keyCodes;

        public Builder(AltUnityKeyCode[] keyCodes) {
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

    public AltUnityKeyCode[] getKeyCode() {
        return keyCodes;
    }

    public void setKeyCode(AltUnityKeyCode[] keyCodes) {
        this.keyCodes = keyCodes;
    }
}
