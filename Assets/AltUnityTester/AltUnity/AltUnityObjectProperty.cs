
using System.Collections.Generic;

public struct AltUnityObjectProperty {

	public string component;
	public string property;

	public AltUnityObjectProperty(string componentName = "", string propertyName = "") {
		this.component = componentName;
		this.property = propertyName;
	}
}
