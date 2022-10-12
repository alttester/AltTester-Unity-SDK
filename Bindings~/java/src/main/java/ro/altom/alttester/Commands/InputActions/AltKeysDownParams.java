package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.UnityStruct.AltKeyCode;

public class AltKeysDownParams extends AltMessage {
    public static class Builder {
        private AltKeyCode[] keyCodes;
        private float power = 1;

        public Builder(AltKeyCode[] keyCodes) {
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

    private AltKeyCode[] keyCodes;
    private float power;

    public AltKeyCode[] getKeyCodes() {
        return keyCodes;
    }

    public void setKeyCodes(AltKeyCode[] keyCodes) {
        this.keyCodes = keyCodes;
    }

    public float getPower() {
        return power;
    }

    public void setPower(float power) {
        this.power = power;
    }

}
