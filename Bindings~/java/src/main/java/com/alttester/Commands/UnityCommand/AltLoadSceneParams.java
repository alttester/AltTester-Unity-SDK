/*
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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
