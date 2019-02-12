
public struct AltUnityObjectProperty {

	public string Component;
	public string Property;
    public string Assembly;

	public AltUnityObjectProperty(string componentName = "", string propertyName = "") {
		Component = componentName;
		Property = propertyName;
	    Assembly = null;
	}

    public AltUnityObjectProperty(string component, string property, string assembly)
    {
        Component = component;
        Property = property;
        Assembly = assembly;
    }
}
