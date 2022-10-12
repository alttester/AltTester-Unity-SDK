package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.UnityStruct.AltKeyCode;

public class AltPressKeyParams extends AltMessage {
    public static class Builder {
        private AltKeyCode keyCode = AltKeyCode.NoKey;
        private float power = 1;
        private float duration = 0.1f;
        private boolean wait = true;

        /**
         * @param keyCode The key code of the key simulated to be pressed.
         */
        public Builder(AltKeyCode keyCode) {
            this.keyCode = keyCode;
        }

        /**
         * 
         * @param duration The time measured in seconds from the key press to the key
         *                 release. Defaults to <code>0.1</code>
         */
        public AltPressKeyParams.Builder withDuration(float duration) {
            this.duration = duration;
            return this;
        }

        /**
         * 
         * @param power A value between [-1,1] used for joysticks to indicate how hard
         *              the button was pressed. Defaults to <code>1</code>
         */
        public AltPressKeyParams.Builder withPower(float power) {
            this.power = power;
            return this;
        }

        /**
         * @param wait If set wait for command to finish. Defaults to <code>true</code>.
         */
        public AltPressKeyParams.Builder withWait(boolean wait) {
            this.wait = wait;
            return this;
        }

        public AltPressKeyParams build() {
            AltPressKeyParams altPressKeyParameters = new AltPressKeyParams();
            altPressKeyParameters.keyCode = this.keyCode;
            altPressKeyParameters.power = this.power;
            altPressKeyParameters.duration = this.duration;
            altPressKeyParameters.wait = this.wait;
            return altPressKeyParameters;
        }
    }

    private AltPressKeyParams() {
    }

    private AltKeyCode keyCode;
    private float power;
    private float duration;
    private boolean wait;

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
