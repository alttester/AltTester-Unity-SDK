package ro.altom.alttester;


public class AltObjectAction {
    public String component;
    public String method;
    public String parameters;
    public String typeOfParameters;
    public String assembly;

    public AltObjectAction(String componentName, String methodName, String parametersNames, String typeOfParameters,String assemblyName) {
        component = componentName;
        method = methodName;
        parameters = parametersNames;
        this.typeOfParameters=typeOfParameters;
        assembly=assemblyName;
    }
}
