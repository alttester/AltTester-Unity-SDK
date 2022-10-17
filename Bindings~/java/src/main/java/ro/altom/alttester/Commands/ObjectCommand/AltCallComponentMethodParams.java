package ro.altom.alttester.Commands.ObjectCommand;

import com.google.gson.Gson;

public class AltCallComponentMethodParams extends AltObjectParams {
    public static class Builder {
        private String componentName;
        private String methodName;
        private Object[] parameters = new Object[] {};
        private String[] typeOfParameters;
        private String assembly;

        public Builder(String componentName, String methodName, Object[] parameters, String assembly) {
            this.componentName = componentName;
            this.methodName = methodName;
            this.parameters = parameters;
            this.assembly = assembly;
        }

        public AltCallComponentMethodParams.Builder withTypeOfParameters(String[] typeOfParameters) {
            this.typeOfParameters = typeOfParameters;
            return this;
        }

        public AltCallComponentMethodParams build() {
            AltCallComponentMethodParams altCallStaticMethodParameters = new AltCallComponentMethodParams();
            altCallStaticMethodParameters.assembly = this.assembly;
            altCallStaticMethodParameters.method = this.methodName;
            if (this.parameters != null) {
                altCallStaticMethodParameters.parameters = new String[this.parameters.length];
                for (int i = 0; i < this.parameters.length; i++) {

                    altCallStaticMethodParameters.parameters[i] = new Gson().toJson(this.parameters[i]);
                }
            }
            altCallStaticMethodParameters.component = this.componentName;
            altCallStaticMethodParameters.typeOfParameters = this.typeOfParameters;
            return altCallStaticMethodParameters;
        }
    }

    private AltCallComponentMethodParams() {
    }

    public String getComponentName() {
        return component;
    }

    public void setComponentName(String componentName) {
        this.component = componentName;
    }

    public String getMethodName() {
        return method;
    }

    public void setMethodName(String methodName) {
        this.method = methodName;
    }

    public Object[] getParameters() {
        return parameters;
    }

    public String[] getTypeOfParameters() {
        return typeOfParameters;
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
