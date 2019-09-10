public class FindElementsWhereNameContainsDriver : AltBaseCommand
{
    string name;
    string cameraName;
    bool enabled;
    public FindElementsWhereNameContainsDriver(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectsWhereNameContains", name, cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }
}