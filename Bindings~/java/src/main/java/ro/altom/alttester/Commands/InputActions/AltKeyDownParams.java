package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.UnityStruct.AltKeyCode;

public class AltKeyDownParams extends AltMessage {
    public static class Builder {
        private AltKeyCode keyCode = AltKeyCode.NoKey;
        private float power = 1;

        public Builder(AltKeyCode keyCode) {
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

    private AltKeyCode keyCode;
    private float power;

    public AltKeyCode getKeyCode() {
        return keyCode;
    }

    public void setKeyCode(AltKeyCode keyCode) {
        this.keyCode = keyCode;
    }

    public float getPower() {
        return power;
    }

    public void setPower(float power) {
        this.power = power;
    }

}
