package ro.altom.altunitytester.Commands.InputActions;

public class AltScrollMouseParameters {
    public static class Builder{
        private float speed=1;
        private float duration =1;
        public Builder(){
        }
        public AltScrollMouseParameters.Builder withDuration(float duration){
            this.duration = duration;
            return this;
        }
        public AltScrollMouseParameters.Builder withSpeed(float speed){
            this.speed= speed;
            return this;
        }
        public AltScrollMouseParameters build(){
            AltScrollMouseParameters altScrollMouseParameters =new AltScrollMouseParameters();
            altScrollMouseParameters.speed=this.speed;
            altScrollMouseParameters.duration =this.duration;
            return altScrollMouseParameters;
        }
    }

    private AltScrollMouseParameters() {
    }

    private float speed;

    public float getSpeed() {
        return speed;
    }

    public void setSpeed(float speed) {
        this.speed = speed;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }

    private float duration;


}
