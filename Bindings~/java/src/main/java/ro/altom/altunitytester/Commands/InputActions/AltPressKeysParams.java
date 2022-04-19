package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltPressKeysParams extends AltMessage {
    public static class Builder {
        private AltUnityKeyCode[] keyCodes;
        private float power = 1;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param keyCodes The key code of the key simulated to be pressed.
         */
        public Builder(AltUnityKeyCode[] keyCodes) {
            this.keyCodes = keyCodes;
        }

        /**
         * 
         * @param duration The time measured in seconds from the key press to the key
         *                 release. Defaults to <code>0.1</code>
         */
        public AltPressKeysParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * 
         * @param power A value between [-1,1] used for joysticks to indicate how hard
         *              the button was pressed. Defaults to <code>1</code>
         */
        public AltPressKeysParams.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltPressKeysParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltPressKeysParams build() {
            AltPressKeysParams altPressKeyParameters = new AltPressKeysParams();
            altPressKeyParameters.keyCodes = this.keyCodes;
            altPressKeyParameters.power = this.power;
            altPressKeyParameters.duration = this.duration;
            altPressKeyParameters.wait = this.wait;
            return altPressKeyParameters;
        }
    }

    private AltPressKeysParams() {
    }

    private AltUnityKeyCode[] keyCodes;
    private float power;
    private float duration;
    private boolean wait;

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

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    public boolean getWait() {
        return this.wait;
    }

    public void setWait(boolean wait) {
        this.wait = wait;
    }

}
