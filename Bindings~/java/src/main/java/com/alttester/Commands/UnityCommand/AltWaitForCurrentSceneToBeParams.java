/*
    Copyright(C) 2025 Altom Consulting

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
