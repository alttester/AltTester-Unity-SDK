package ro.altom.altunitytester.Commands.OldFindObject;

public class AltWaitForElementWithTextParameters {
    public static class Builder{
        private AltFindElementsParameters altFindElementsParameters;
        private double timeout=20;
        private double interval=0.5;
        private String text;
        public Builder(AltFindElementsParameters altFindElementsParameters,String text){
            this.altFindElementsParameters=altFindElementsParameters;
            this.text=text;
        }
        public AltWaitForElementWithTextParameters.Builder withTimeout(double timeout){
            this.timeout= timeout;
            return this;
        }
        public AltWaitForElementWithTextParameters.Builder withInterval(double interval){
            this.interval=interval;
            return this;
        }
        public AltWaitForElementWithTextParameters build(){
            AltWaitForElementWithTextParameters altWaitForObjectsParameters =new AltWaitForElementWithTextParameters();
            altWaitForObjectsParameters.altFindElementsParameters=this.altFindElementsParameters;
            altWaitForObjectsParameters.timeout=this.timeout;
            altWaitForObjectsParameters.interval=this.interval;
            altWaitForObjectsParameters.text=this.text;
            return altWaitForObjectsParameters;
        }
    }

    private AltWaitForElementWithTextParameters() {
    }

    private AltFindElementsParameters altFindElementsParameters;
    private double timeout=20;
    private double interval=0.5;

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    private String text;

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

    public AltFindElementsParameters getAltFindElementsParameters() {
        return altFindElementsParameters;
    }

    public void setAltFindElementsParameters(AltFindElementsParameters altFindElementsParameters) {
        this.altFindElementsParameters = altFindElementsParameters;
    }
}
