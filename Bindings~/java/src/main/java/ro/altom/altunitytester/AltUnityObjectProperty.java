package ro.altom.altunitytester;

public class AltUnityObjectProperty {
    public String component;
    public String property;
    public String assembly;

    public AltUnityObjectProperty(String assemblyName,String componentName, String propertyName) {
        assembly=assemblyName;
        component = componentName;
        property = propertyName;
    }

    public void setAssembly(String assembly){
        this.assembly = assembly;
    }
}
