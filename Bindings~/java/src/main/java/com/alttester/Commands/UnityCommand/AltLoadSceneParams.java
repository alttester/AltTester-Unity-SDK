package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;

public class AltLoadSceneParams extends AltMessage {
    private boolean loadSingle = true;
    private String sceneName;

    public static class Builder {
        private String sceneName;
        private boolean loadSingle = true;

        public Builder(String sceneName) {
            this.sceneName = sceneName;
        }

        public AltLoadSceneParams.Builder loadSingle(boolean loadSingle) {
            this.loadSingle = loadSingle;
            return this;
        }

        public AltLoadSceneParams build() {
            AltLoadSceneParams altLoadSceneParameters = new AltLoadSceneParams();
            altLoadSceneParameters.loadSingle = this.loadSingle;
            altLoadSceneParameters.sceneName = this.sceneName;
            return altLoadSceneParameters;
        }
    }

    private AltLoadSceneParams() {
    }

    public String getSceneName() {
        return sceneName;
    }

    public void setSceneName(String sceneName) {
        this.sceneName = sceneName;
    }

    public boolean getLoadSingle() {
        return loadSingle;
    }

    public void setLoadSingle(boolean loadSingle) {
        this.loadSingle = loadSingle;
    }
}
