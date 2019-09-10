public class FindElementsDriver : AltBaseCommand
{
    string name;
    string cameraName;
    bool enabled;
    public FindElementsDriver(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute(){
        Socket.Client.Send(toBytes(CreateCommand("findObjectsByName", name, cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }
}