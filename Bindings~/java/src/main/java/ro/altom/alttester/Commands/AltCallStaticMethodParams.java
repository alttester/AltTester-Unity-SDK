package ro.altom.alttester.Commands;

import com.google.gson.Gson;

import ro.altom.alttester.AltMessage;

public class AltCallStaticMethodParams extends AltMessage {
    public static class Builder {
        private String component;
        private String method;
        private Object[] parameters = new Object[] {};
        private String[] typeOfParameters;
        private String assembly;

        public Builder(String component, String method, Object[] parameters, String assembly) {
            this.component = component;
            this.method = method;
            this.parameters = parameters;
            this.assembly = assembly;
        }

        public Builder withTypeOfParameters(String[] typeOfParameters) {
            this.typeOfParameters = typeOfParameters;
            return this;
        }

        public AltCallStaticMethodParams build() {
            AltCallStaticMethodParams altCallStaticMethodParameters = new AltCallStaticMethodParams();
            altCallStaticMethodParameters.assembly = this.assembly;
            altCallStaticMethodParameters.method = this.method;
            if (this.parameters != null) {
                altCallStaticMethodParameters.parameters = new String[this.parameters.length];
                for (int i = 0; i < this.parameters.length; i++) {

                    altCallStaticMethodParameters.parameters[i] = new Gson().toJson(this.parameters[i]);
                }
            }
            altCallStaticMethodParameters.component = this.component;
            altCallStaticMethodParameters.typeOfParameters = this.typeOfParameters;
            return altCallStaticMethodParameters;
        }
    }

    private AltCallStaticMethodParams() {
    }

    public String getComponent() {
        return component;
    }

    public void setComponent(String typeName) {
        this.component = typeName;
    }

    public String getMethod() {
        return method;
    }

    public void setMethod(String methodName) {
        this.method = methodName;
    }

    public Object[] getParameters() {
        return parameters;
    }

    public String[] getTypeOfParameters() {
        return typeOfParameters;
    }

    public void setTypeOfParameters(String[] typeOfParameters) {
        this.typeOfParameters = typeOfParameters;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String component;
    private String method;
    private String[] parameters;
    private String[] typeOfParameters;
    private String assembly;
}
