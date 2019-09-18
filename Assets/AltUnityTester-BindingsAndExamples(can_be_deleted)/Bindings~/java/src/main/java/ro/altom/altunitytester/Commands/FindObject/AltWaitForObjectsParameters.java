package ro.altom.altunitytester.Commands.FindObject;

public class AltWaitForObjectsParameters {
    public static class Builder{
        private AltFindObjectsParameters altFindObjectsParameters;
        private double timeout=20;
        private double interval=0.5;
        public Builder(AltFindObjectsParameters altFindObjectsParameters){
            this.altFindObjectsParameters=altFindObjectsParameters;
        }
        public AltWaitForObjectsParameters.Builder withTimeout(double timeout){
            this.timeout= timeout;
            return this;
        }
        public AltWaitForObjectsParameters.Builder withInterval(double interval){
            this.interval=interval;
            return this;
        }
        public AltWaitForObjectsParameters build(){
            AltWaitForObjectsParameters altWaitForObjectsParameters =new AltWaitForObjectsParameters();
            altWaitForObjectsParameters.altFindObjectsParameters=this.altFindObjectsParameters;
            altWaitForObjectsParameters.timeout=this.timeout;
            altWaitForObjectsParameters.interval=this.interval;
            return altWaitForObjectsParameters;
        }
    }

    private AltWaitForObjectsParameters() {
    }

    private AltFindObjectsParameters altFindObjectsParameters;
    private double timeout=20;
    private double interval=0.5;

    public double getTimeout() {
        return timeout;
    }

    public void setTimeout(double timeout) {
        this.timeout = timeout;
    }

    public double getInterval() {
        return interval;
    }

    public void setInterval(double interval) {
        this.interval = interval;
    }

    public AltFindObjectsParameters getAltFindObjectsParameters() {
        return altFindObjectsParameters;
    }

    public void setAltFindObjectsParameters(AltFindObjectsParameters altFindObjectsParameters) {
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
}
