package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;

public class AltSetTimeScaleParams extends AltMessage {

    private float timeScale;

    public static class Builder {
        private float timeScale;

        public Builder(float timeScale) {
            this.timeScale = timeScale;
        }

        public AltSetTimeScaleParams build() {
            AltSetTimeScaleParams params = new AltSetTimeScaleParams();
            params.timeScale = this.timeScale;
            return params;
        }
    }

    private AltSetTimeScaleParams() {
    }

    public float getTimeScale() {
        return timeScale;
    }

    public void setTimeScale(float timeScale) {
        this.timeScale = timeScale;
    }
}
