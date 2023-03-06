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
        private AltObject obj;

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
            altWaitForComponentPropertyParams.obj = this.obj;

            return altWaitForComponentPropertyParams;
        }
    }

    private AltWaitForComponentPropertyParams() {
    }

    private AltGetComponentPropertyParams altGetComponentPropertyParams;
    private T propertyValue;
    private double timeout = 20;
    private double interval = 0.5;
    private AltObject obj;

    public AltGetComponentPropertyParams getAltGetComponentPropertyParams() {
        return altGetComponentPropertyParams;
    }

    public void setAltGetComponentPropertyParams(AltGetComponentPropertyParams altGetComponentPropertyParams) {
        this.altGetComponentPropertyParams = altGetComponentPropertyParams;
    }

    public AltObject getAltObject() {
        return obj;
    }

    public void setAltObject(AltObject obj) {
        this.obj = obj;
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