public class AltUnityGetAllElements : AltUnityBaseFindObjects
{
    By cameraBy;
    string cameraPath;
    bool enabled;
    public AltUnityGetAllElements(SocketSettings socketSettings,By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
    {
        this.cameraBy = cameraBy;
        this.cameraPath = cameraPath;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute()
    {
        cameraPath = SetPath(cameraBy, cameraPath);
        Socket.Client.Send(toBytes(CreateCommand("findObjects", "//*", cameraBy.ToString(),cameraPath, enabled.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }
}