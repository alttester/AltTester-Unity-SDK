public class FindElementsByComponentDriver : AltBaseCommand
{
    string componentName;
    string assemblyName;
    string cameraName;
    bool enabled;
    public FindElementsByComponentDriver(SocketSettings socketSettings, string componentName, string assemblyName, string cameraName, bool enabled) : base(socketSettings)
    {
        this.componentName = componentName;
        this.assemblyName = assemblyName;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectsByComponent", assemblyName, componentName, cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }
}