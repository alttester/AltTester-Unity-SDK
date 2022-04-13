package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeysDownParams extends AltMessage {
    public static class Builder {
        private AltUnityKeyCode[] keyCodes;
        private float power = 1;

        public Builder(AltUnityKeyCode[] keyCodes) {
            this.keyCodes = keyCodes;
        }

        public AltKeysDownParams.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        public AltKeysDownParams build() {
            AltKeysDownParams params = new AltKeysDownParams();
            params.keyCodes = this.keyCodes;
            params.power = this.power;
            return params;
        }
    }

    private AltKeysDownParams() {
        this.setCommandName("keysDown");
    }

    private AltUnityKeyCode[] keyCodes;
    private float power;

    public AltUnityKeyCode[] getKeyCodes() {
        return keyCodes;
    }

    public void setKeyCodes(AltUnityKeyCode[] keyCodes) {
        this.keyCodes = keyCodes;
    }

    public float getPower() {
        return power;
    }

    public void setPower(float power) {
        this.power = power;
    }

}
