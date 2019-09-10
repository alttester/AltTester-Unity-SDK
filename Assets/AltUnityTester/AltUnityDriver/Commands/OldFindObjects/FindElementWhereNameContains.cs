public class FindElementWhereNameContainsDriver : AltBaseCommand
{
    string name;
    string cameraName;
    bool enabled;
    public FindElementWhereNameContainsDriver(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectWhereNameContains", name, cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            if (altElement.name.Contains(name))
            {
                return altElement;
            }
        }
        HandleErrors(data);
        return null;
    }
}