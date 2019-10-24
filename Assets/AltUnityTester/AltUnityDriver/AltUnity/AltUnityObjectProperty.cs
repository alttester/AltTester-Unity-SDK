public struct AltUnityObjectProperty
{ 
    public string Component;
    public string Property;
    public string Assembly;

    public AltUnityObjectProperty(string component = "", string property = "") :
        this(component, property, null) { }

    public AltUnityObjectProperty(string component, string property, string assembly)
    {
        Component = component;
        Property = property;
        Assembly = assembly;
    }
}