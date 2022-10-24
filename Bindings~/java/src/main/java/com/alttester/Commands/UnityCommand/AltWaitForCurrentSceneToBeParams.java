package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;

public class AltWaitForCurrentSceneToBeParams extends AltMessage {
    public static class Builder {
        private String sceneName;
        private double timeout = 20;
        private double interval = 0.5;

        public Builder(String sceneName) {
            this.sceneName = sceneName;
        }

        public AltWaitForCurrentSceneToBeParams.Builder withTimeout(double timeout) {
            this.timeout = timeout;
            return this;
        }

        public AltWaitForCurrentSceneToBeParams.Builder withInterval(double interval) {
            this.interval = interval;
            return this;
        }

        public AltWaitForCurrentSceneToBeParams build() {
            AltWaitForCurrentSceneToBeParams altWaitForCurrentSceneToBeParameters = new AltWaitForCurrentSceneToBeParams();
            altWaitForCurrentSceneToBeParameters.timeout = this.timeout;
            altWaitForCurrentSceneToBeParameters.interval = this.interval;
            altWaitForCurrentSceneToBeParameters.sceneName = this.sceneName;
            return altWaitForCurrentSceneToBeParameters;
        }
    }

    private AltWaitForCurrentSceneToBeParams() {
    }

    private double timeout = 20;
    private double interval = 0.5;

    public String getSceneName() {
        return sceneName;
    }

    public void setSceneName(String sceneName) {
        this.sceneName = sceneName;
    }

    private String sceneName;

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

}
