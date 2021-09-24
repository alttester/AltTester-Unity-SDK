package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;

public class AltSetTimescaleParams extends AltMessage{
    
    private float timeScale;

    public AltSetTimescaleParams(float timeScale){
        this.setTimeScale(timeScale);
    }

    public float getTimeScale() {
        return timeScale;
    }

    public void setTimeScale(float timeScale) {
        this.timeScale = timeScale;
    }
}
