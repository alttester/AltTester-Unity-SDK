public class FindElementDriver : AltBaseCommand
{
    string name;
    string cameraName;
    bool enabled;
    public FindElementDriver(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute(){
        Socket.Client.Send(toBytes(CreateCommand("findObjectByName", name, cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
        }
        HandleErrors(data);
        return null;
    }
}