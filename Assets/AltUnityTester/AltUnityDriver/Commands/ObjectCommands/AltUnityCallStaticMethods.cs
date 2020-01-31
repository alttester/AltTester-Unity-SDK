public class AltUnityCallStaticMethods : AltBaseCommand
{
    string typeName;
    string methodName;
    string parameters;
    string typeOfParameters;
    string assemblyName;
    public AltUnityCallStaticMethods(SocketSettings socketSettings, string typeName, string methodName, string parameters, string typeOfParameters, string assemblyName) : base(socketSettings)
    {
        this.typeName = typeName;
        this.methodName = methodName;
        this.parameters = parameters;
        this.typeOfParameters = typeOfParameters;
        this.assemblyName = assemblyName;
    }
    public string Execute(){
        string actionInfo =
            Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectAction(typeName, methodName, parameters, typeOfParameters, assemblyName));
        Socket.Client.Send(toBytes(CreateCommand("callComponentMethodForObject", "", actionInfo)));
        var data = Recvall();
        if (!data.Contains("error:")) return data;
        HandleErrors(data);
        return null;
    }
}