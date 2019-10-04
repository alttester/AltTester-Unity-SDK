package ro.altom.altunitytester.Commands.FindObject;

public class AltWaitForObjectWithTextParameters {
    public static class Builder{
        private AltFindObjectsParameters altFindObjectsParameters;
        private double timeout=20;
        private double interval=0.5;
        private String text;
        public Builder(AltFindObjectsParameters altFindObjectsParameters,String text){
            this.altFindObjectsParameters=altFindObjectsParameters;
            this.text=text;
        }
        public AltWaitForObjectWithTextParameters.Builder withTimeout(double timeout){
            this.timeout= timeout;
            return this;
        }
        public AltWaitForObjectWithTextParameters.Builder withInterval(double interval){
            this.interval=interval;
            return this;
        }
        public AltWaitForObjectWithTextParameters build(){
            AltWaitForObjectWithTextParameters altWaitForObjectWithTextParameters =new AltWaitForObjectWithTextParameters();
            altWaitForObjectWithTextParameters.altFindObjectsParameters=this.altFindObjectsParameters;
            altWaitForObjectWithTextParameters.timeout=this.timeout;
            altWaitForObjectWithTextParameters.interval=this.interval;
            altWaitForObjectWithTextParameters.text=this.text;
            return altWaitForObjectWithTextParameters;
        }
    }

    private AltWaitForObjectWithTextParameters() {
    }

    private AltFindObjectsParameters altFindObjectsParameters;
    private String text;

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

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
