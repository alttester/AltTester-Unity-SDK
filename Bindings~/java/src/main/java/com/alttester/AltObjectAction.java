package com.alttester;

public class AltObjectAction {
    public String component;
    public String method;
    public String parameters;
    public String typeOfParameters;
    public String assembly;

    public AltObjectAction(final String componentName, final String methodName, final String parametersNames,
            final String typeOfParameters,
            final String assemblyName) {
        this.component = componentName;
        this.method = methodName;
        this.parameters = parametersNames;
        this.typeOfParameters = typeOfParameters;
        this.assembly = assemblyName;
    }
}
