package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;

public class AltLoadSceneParameters extends AltMessage {
    public static class Builder {
        private String sceneName;
        private boolean loadSingle = true;

        public Builder(String sceneName) {
            this.sceneName = sceneName;
        }

        public AltLoadSceneParameters.Builder loadMode(boolean loadSingle) {
            this.loadSingle = loadSingle;
            return this;
        }

        public AltLoadSceneParameters build() {
            AltLoadSceneParameters altLoadSceneParameters = new AltLoadSceneParameters();
            altLoadSceneParameters.loadSingle = this.loadSingle;
            altLoadSceneParameters.sceneName = this.sceneName;
            return altLoadSceneParameters;
        }
    }

    private AltLoadSceneParameters() {
    }

    private boolean loadSingle = true;

    public String getSceneName() {
        return sceneName;
    }

    public void setSceneName(String sceneName) {
        this.sceneName = sceneName;
    }

    private String sceneName;

    public boolean getLoadSingle() {
        return loadSingle;
    }

    public void setLoadSingle(boolean loadSingle) {
        this.loadSingle = loadSingle;
    }
}
