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

package com.alttester.Commands.FindObject;

import com.alttester.AltMessage;
import com.alttester.AltObject;
import com.alttester.Commands.ObjectCommand.AltGetComponentPropertyParams;

public class AltWaitForComponentPropertyParams<T> extends AltMessage {
    public static class Builder<T> {
        private AltGetComponentPropertyParams altGetComponentPropertyParams;
        private double timeout = 20;
        private double interval = 0.5;
        private T propertyValue;
        private AltObject altObject;

        public Builder(AltGetComponentPropertyParams altGetComponentPropertyParams) {
            this.altGetComponentPropertyParams = altGetComponentPropertyParams;
        }

        public AltWaitForComponentPropertyParams.Builder<T> withTimeout(double timeout) {
            this.timeout = timeout;
            return this;
        }

        public AltWaitForComponentPropertyParams.Builder<T> withInterval(double interval) {
            this.interval = interval;
            return this;
        }

        public AltWaitForComponentPropertyParams<T> build() {
            AltWaitForComponentPropertyParams<T> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams<T>();
            altWaitForComponentPropertyParams.altGetComponentPropertyParams = altGetComponentPropertyParams;
            altWaitForComponentPropertyParams.timeout = this.timeout;
            altWaitForComponentPropertyParams.interval = this.interval;
            altWaitForComponentPropertyParams.propertyValue = this.propertyValue;
            altWaitForComponentPropertyParams.altObject = this.altObject;

            return altWaitForComponentPropertyParams;
        }
    }

    private AltWaitForComponentPropertyParams() {
    }

    private AltGetComponentPropertyParams altGetComponentPropertyParams;
    private T propertyValue;
    private double timeout = 20;
    private double interval = 0.5;
    private AltObject altObject;

    public AltGetComponentPropertyParams getAltGetComponentPropertyParams() {
        return altGetComponentPropertyParams;
    }

    public void setAltGetComponentPropertyParams(AltGetComponentPropertyParams altGetComponentPropertyParams) {
        this.altGetComponentPropertyParams = altGetComponentPropertyParams;
    }

    public AltObject getAltObject() {
        return altObject;
    }

    public void setAltObject(AltObject altObject) {
        this.altObject = altObject;
    }

    public T getPropertyValue() {
        return propertyValue;
    }

    public void setPropertyValue(T propertyValue) {
        this.propertyValue = propertyValue;
    }

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
