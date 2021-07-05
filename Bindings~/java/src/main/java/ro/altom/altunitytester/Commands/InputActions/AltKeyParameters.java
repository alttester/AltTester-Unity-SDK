package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeyParameters {
    public static class Builder {
        private AltUnityKeyCode keyCode = AltUnityKeyCode.NoKey;
        private float power = 1;

        public Builder(AltUnityKeyCode keyCode) {
            this.keyCode = keyCode;
        }

        public AltKeyParameters.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        public AltKeyParameters build() {
            AltKeyParameters altKeyUpParameters = new AltKeyParameters();
            altKeyUpParameters.keyCode = this.keyCode;
            altKeyUpParameters.power = this.power;
            return altKeyUpParameters;
        }
    }

    private AltKeyParameters() {
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
