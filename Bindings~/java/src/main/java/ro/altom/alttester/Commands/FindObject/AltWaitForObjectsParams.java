package ro.altom.alttester.Commands.FindObject;

import ro.altom.alttester.AltMessage;

public class AltWaitForObjectsParams extends AltMessage {
    public static class Builder {
        private AltFindObjectsParams altFindObjectsParameters;
        private double timeout = 20;
        private double interval = 0.5;

        public Builder(AltFindObjectsParams altFindObjectsParameters) {
            this.altFindObjectsParameters = altFindObjectsParameters;
        }

        public AltWaitForObjectsParams.Builder withTimeout(double timeout) {
            this.timeout = timeout;
            return this;
        }

        public AltWaitForObjectsParams.Builder withInterval(double interval) {
            this.interval = interval;
            return this;
        }

        public AltWaitForObjectsParams build() {
            AltWaitForObjectsParams altWaitForObjectsParameters = new AltWaitForObjectsParams();
            altWaitForObjectsParameters.altFindObjectsParameters = this.altFindObjectsParameters;
            altWaitForObjectsParameters.timeout = this.timeout;
            altWaitForObjectsParameters.interval = this.interval;
            return altWaitForObjectsParameters;
        }
    }

    private AltWaitForObjectsParams() {
    }

    private AltFindObjectsParams altFindObjectsParameters;
    private double timeout = 20;
    private double interval = 0.5;

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

    public AltFindObjectsParams getAltFindObjectsParameters() {
        return altFindObjectsParameters;
    }

    public void setAltFindObjectsParameters(AltFindObjectsParams altFindObjectsParameters) {
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
}
