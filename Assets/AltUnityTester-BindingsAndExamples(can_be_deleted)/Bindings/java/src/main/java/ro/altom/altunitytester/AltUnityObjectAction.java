package ro.altom.altunitytester;

public class AltUnityObjectAction {
    public String component;
    public String method;
    public String parameters;
    public String assembly;

    public AltUnityObjectAction(String assemblyName,String componentName, String methodName, String parametersNames) {
        component = componentName;
        method = methodName;
        parameters = parametersNames;
        assembly=assembly;
    }
}
