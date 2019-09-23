public class GetAllElements : AltBaseCommand
{
    string cameraName;
    bool enabled;
    public GetAllElements(SocketSettings socketSettings, string cameraName, bool enabled) : base(socketSettings)
    {
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjects", "//*", cameraName, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }
}