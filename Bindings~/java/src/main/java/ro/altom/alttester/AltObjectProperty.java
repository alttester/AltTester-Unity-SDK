package ro.altom.alttester;

public class AltObjectProperty {
    public String component;
    public String property;
    public String assembly;

    public AltObjectProperty(String assemblyName,String componentName, String propertyName) {
        assembly=assemblyName;
        component = componentName;
        property = propertyName;
    }

    public void setAssembly(String assembly){
        this.assembly = assembly;
    }
}
