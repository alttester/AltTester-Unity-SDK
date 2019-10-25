package ro.altom.altunitytester.Commands.OldFindObject;

import ro.altom.altunitytester.Commands.FindObject.AltFindObjectsParameters;
import ro.altom.altunitytester.Commands.FindObject.AltWaitForObjectsParameters;

public class AltWaitForElementParameters {
    public static class Builder{
        private AltFindElementsParameters altFindElementsParameters;
        private double timeout=20;
        private double interval=0.5;
        public Builder(AltFindElementsParameters altFindElementsParameters){
            this.altFindElementsParameters=altFindElementsParameters;
        }
        public AltWaitForElementParameters.Builder withTimeout(double timeout){
            this.timeout= timeout;
            return this;
        }
        public AltWaitForElementParameters.Builder withInterval(double interval){
            this.interval=interval;
            return this;
        }
        public AltWaitForElementParameters build(){
            AltWaitForElementParameters altFindElementsParameters =new AltWaitForElementParameters();
            altFindElementsParameters.altFindObjectsParameters=this.altFindElementsParameters;
            altFindElementsParameters.timeout=this.timeout;
            altFindElementsParameters.interval=this.interval;
            return altFindElementsParameters;
        }
    }

    private AltWaitForElementParameters() {
    }

    private AltFindElementsParameters altFindObjectsParameters;
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

    public AltFindElementsParameters getAltFindObjectsParameters() {
        return altFindObjectsParameters;
    }

    public void setAltFindObjectsParameters(AltFindElementsParameters altFindObjectsParameters) {
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
}
