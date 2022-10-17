package ro.altom.alttester.Commands.ObjectCommand;

import com.google.gson.Gson;

public class AltSetComponentPropertyParams extends AltObjectParams {
    public static class Builder {
        private String componentName;
        private String propertyName;
        private String assembly;
        private String value;

        public Builder(String componentName, String propertyName, Object value, String assembly) {
            this.componentName = componentName;
            this.propertyName = propertyName;
            this.assembly = assembly;
            this.value = new Gson().toJson(value);
        }

        public AltSetComponentPropertyParams build() {
            AltSetComponentPropertyParams altSetComponentPropertyParameters = new AltSetComponentPropertyParams();
            altSetComponentPropertyParameters.assembly = this.assembly;
            altSetComponentPropertyParameters.property = this.propertyName;
            altSetComponentPropertyParameters.component = this.componentName;
            altSetComponentPropertyParameters.value = this.value;

            return altSetComponentPropertyParameters;
        }
    }

    private AltSetComponentPropertyParams() {
    }

    public String getComponentName() {
        return component;
    }

    public void setComponentName(String componentName) {
        this.component = componentName;
    }

    public String getPropertyName() {
        return property;
    }

    public void setPropertyName(String propertyName) {
        this.property = propertyName;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String component;
    private String property;
    private String assembly;

    public String getValue() {
        return value;
    }

    public void setValue(Object value) {
        this.value = new Gson().toJson(value);
    }

    private String value;
}
