package ro.altom.altunitytester;


public class AltUnityObjectAction {
    public String component;
    public String method;
    public String parameters;
    public String typeOfParameters;
    public String assembly;

    public AltUnityObjectAction(String componentName, String methodName, String parametersNames, String typeOfParameters,String assemblyName) {
        component = componentName;
        method = methodName;
        parameters = parametersNames;
        this.typeOfParameters=typeOfParameters;
        assembly=assemblyName;




    }
}
