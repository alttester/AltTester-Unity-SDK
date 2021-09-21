package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltPressKeyParameters extends AltMessage{
    public static class Builder {
        private AltUnityKeyCode keyCode = AltUnityKeyCode.NoKey;
        @Deprecated
        private String keyName = "";
        private float power = 1;
        private float duration = 1;

        public Builder(AltUnityKeyCode keyCode) {
            this.keyCode = keyCode;
        }

        @Deprecated
        public Builder(String keyName) {
            this.keyName = keyName;
        }

        public AltPressKeyParameters.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        public AltPressKeyParameters.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        public AltPressKeyParameters build() {
            AltPressKeyParameters altPressKeyParameters = new AltPressKeyParameters();
            altPressKeyParameters.keyCode = this.keyCode;
            altPressKeyParameters.keyName = this.keyName;
            altPressKeyParameters.power = this.power;
            altPressKeyParameters.duration = this.duration;
            return altPressKeyParameters;
        }
    }

    private AltPressKeyParameters() {
    }

    private AltUnityKeyCode keyCode;
    @Deprecated
    private String keyName;
    private float power;

    public AltUnityKeyCode getKeyCode() {
        return keyCode;
    }

    @Deprecated
    public String getKeyName() {
        return this.keyName;
    }

    public void setKeyCode(AltUnityKeyCode keyCode) {
        this.keyCode = keyCode;
    }

    @Deprecated
    public void setKeyName(String keyName) {
        this.keyName = keyName;
    }

    public float getPower() {
        return power;
    }

    public void setPower(float power) {
        this.power = power;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    private float duration;

}
