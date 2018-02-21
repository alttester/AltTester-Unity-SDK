
using System.Collections.Generic;

public struct AltUnityObjectAction {

	public string component;
	public string method;
	// public Dictionary<string, string> parameters;
	public string parameters;

	public AltUnityObjectAction(string componentName = "", string methodName = "", string parameters = null) {
		this.component = componentName;
		this.method = methodName;
		this.parameters = parameters;
	}
}
