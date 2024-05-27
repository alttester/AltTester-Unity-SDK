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
