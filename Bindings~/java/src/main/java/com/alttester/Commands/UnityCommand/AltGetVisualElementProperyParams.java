package com.alttester.Commands.UnityCommand;

import com.alttester.Commands.ObjectCommand.AltObjectParams;

public class AltGetVisualElementProperyParams extends AltObjectParams {
    private String property;

    public static class Builder {
        private String propertyName;

        public Builder(String propertyName) {
            this.propertyName = propertyName;
        }

        public AltGetVisualElementProperyParams build() {
            AltGetVisualElementProperyParams altUnloadSceneParams = new AltGetVisualElementProperyParams();
            altUnloadSceneParams.property = this.propertyName;
            return altUnloadSceneParams;
        }
    }

    private AltGetVisualElementProperyParams() {
        super();
    }

    public String getProperty() {
        return property;
    }

    public void setProperty(String propertyName) {
        this.property = propertyName;
    }
}
