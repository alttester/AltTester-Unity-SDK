package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;

public class AltUnloadSceneParams extends AltMessage {
    private String sceneName;

    public static class Builder {
        private String sceneName;

        public Builder(String sceneName) {
            this.sceneName = sceneName;
        }

        public AltUnloadSceneParams build() {
            AltUnloadSceneParams altUnloadSceneParams = new AltUnloadSceneParams();
            altUnloadSceneParams.sceneName = this.sceneName;
            return altUnloadSceneParams;
        }
    }

    private AltUnloadSceneParams() {
    }

    public String getSceneName() {
        return sceneName;
    }

    public void setSceneName(String sceneName) {
        this.sceneName = sceneName;
    }
}
