public class AltUnityCallComponentMethod : AltBaseCommand
{
    string componentName;
    string methodName;
    string parameters;
    string typeOfParameters;
    string assemblyName;
    AltUnityObject altUnityObject;
    public AltUnityCallComponentMethod(SocketSettings socketSettings, string componentName, string methodName, string parameters, string typeOfParameters, string assembly, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.componentName = componentName;
        this.methodName = methodName;
        this.parameters = parameters;
        this.typeOfParameters = typeOfParameters;
        this.assemblyName = assembly;
        this.altUnityObject = altUnityObject;
    }
    public string Execute()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        string actionInfo =
            Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectAction(componentName, methodName, parameters, typeOfParameters, assemblyName));
        Socket.Client.Send(
             System.Text.Encoding.ASCII.GetBytes(CreateCommand("callComponentMethodForObject", altObject, actionInfo)));
        string data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;

    }
}