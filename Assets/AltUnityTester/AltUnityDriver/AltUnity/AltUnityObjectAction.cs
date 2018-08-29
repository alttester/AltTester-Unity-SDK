
public struct AltUnityObjectAction {

	public string Component;
	public string Method;
	// public Dictionary<string, string> parameters;
	public string Parameters;
    public string TypeOfParameters;

    public AltUnityObjectAction(string component, string method, string parameters, string typeOfParameters)
    {
        Component = component;
        Method = method;
        Parameters = parameters;
        TypeOfParameters = typeOfParameters;
    }

    public AltUnityObjectAction(string component, string method, string parameters) : this()
    {
        Component = component;
        Method = method;
        Parameters = parameters;
        TypeOfParameters = "";
    }
}
