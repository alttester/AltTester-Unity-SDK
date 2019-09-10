public class FindElementByComponentDriver : AltBaseCommand
{
    string componentName;
    string assemblyName;
    string cameraName;
    bool enabled;
    public FindElementByComponentDriver(SocketSettings socketSettings, string componentName, string assemblyName, string cameraName, bool enabled) : base(socketSettings)
    {
        this.componentName = componentName;
        this.assemblyName = assemblyName;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute(){
        Socket.Client.Send(toBytes(CreateCommand("findObjectByComponent", assemblyName, componentName, cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
        }
        HandleErrors(data);
        return null;
    }
}