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

package com.alttester.Commands.FindObject;

import com.alttester.AltMessage;

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
