package ro.altom.altunitytester.Commands.InputActions;

public class AltPressKeyParameters {
    public static class Builder{
        private String keyName;
        private float power=1;
        private float duration =1;
        public Builder(String keyName){
            this.keyName=keyName;
        }
        public AltPressKeyParameters.Builder withDuration(float duration){
            this.duration = duration;
            return this;
        }
        public AltPressKeyParameters.Builder withPower(float power){
            this.power = power;
            return this;
        }
        public AltPressKeyParameters build(){
            AltPressKeyParameters altPressKeyParameters=new AltPressKeyParameters();
            altPressKeyParameters.keyName=this.keyName;
            altPressKeyParameters.power=this.power;
            altPressKeyParameters.duration =this.duration;
            return altPressKeyParameters;
        }
    }

    private AltPressKeyParameters() {
    }

    private String keyName;
    private float power;

    public String getKeyName() {
        return keyName;
    }

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
