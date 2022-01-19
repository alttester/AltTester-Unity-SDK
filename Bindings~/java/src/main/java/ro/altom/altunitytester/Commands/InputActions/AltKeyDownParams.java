package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeyDownParams extends AltMessage {
    public static class Builder {
        private AltUnityKeyCode keyCode = AltUnityKeyCode.NoKey;
        private float power = 1;

        public Builder(AltUnityKeyCode keyCode) {
            this.keyCode = keyCode;
        }

        public AltKeyDownParams.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        public AltKeyDownParams build() {
            AltKeyDownParams params = new AltKeyDownParams();
            params.keyCode = this.keyCode;
            params.power = this.power;
            return params;
        }
    }

    private AltKeyDownParams() {
        this.setCommandName("keyDown");
    }

    private AltUnityKeyCode keyCode;
    private float power;

    public AltUnityKeyCode getKeyCode() {
        return keyCode;
    }

    public void setKeyCode(AltUnityKeyCode keyCode) {
        this.keyCode = keyCode;
    }

    public float getPower() {
        return power;
    }

    public void setPower(float power) {
        this.power = power;
    }

}
