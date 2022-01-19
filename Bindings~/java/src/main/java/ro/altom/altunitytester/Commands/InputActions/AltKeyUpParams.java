package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeyUpParams extends AltMessage {

    private AltUnityKeyCode keyCode;

    public static class Builder {
        private AltUnityKeyCode keyCode = AltUnityKeyCode.NoKey;

        public Builder(AltUnityKeyCode keyCode) {
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

    public AltUnityKeyCode getKeyCode() {
        return keyCode;
    }

    public void setKeyCode(AltUnityKeyCode keyCode) {
        this.keyCode = keyCode;
    }
}
