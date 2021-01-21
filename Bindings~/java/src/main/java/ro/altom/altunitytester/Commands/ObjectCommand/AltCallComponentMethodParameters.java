package ro.altom.altunitytester.Commands.ObjectCommand;

public class AltCallComponentMethodParameters {
    public static class Builder {
        private String componentName;
        private String methodName;
        private String parameters;
        private String typeOfParameters;
        private String assembly;

        public Builder(String componentName, String methodName, String parameters) {
            this.componentName = componentName;
            this.methodName = methodName;
            this.parameters = parameters;
        }

        public AltCallComponentMethodParameters.Builder withAssembly(String assembly) {
            this.assembly = assembly;
            return this;
        }

        public AltCallComponentMethodParameters.Builder withTypeOfParameters(String typeOfParameters) {
            this.typeOfParameters = typeOfParameters;
            return this;
        }

        public AltCallComponentMethodParameters build() {
            AltCallComponentMethodParameters altCallStaticMethodParameters = new AltCallComponentMethodParameters();
            altCallStaticMethodParameters.assembly = this.assembly;
            altCallStaticMethodParameters.methodName = this.methodName;
            altCallStaticMethodParameters.parameters = this.parameters;
            altCallStaticMethodParameters.componentName = this.componentName;
            altCallStaticMethodParameters.typeOfParameters = this.typeOfParameters;
            return altCallStaticMethodParameters;
        }
    }

    private AltCallComponentMethodParameters() {
    }

    public String getComponentName() {
        return componentName;
    }

    public void setComponentName(String componentName) {
        this.componentName = componentName;
    }

    public String getMethodName() {
        return methodName;
    }

    public void setMethodName(String methodName) {
        this.methodName = methodName;
    }

    public String getParameters() {
        return parameters;
    }

    public void setParameters(String parameters) {
        this.parameters = parameters;
    }

    public String getTypeOfParameters() {
        return typeOfParameters;
    }

    public void setTypeOfParameters(String typeOfParameters) {
        this.typeOfParameters = typeOfParameters;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String componentName;
    private String methodName;
    private String parameters;
    private String typeOfParameters;
    private String assembly;
}
