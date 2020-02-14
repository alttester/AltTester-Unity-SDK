public class AltUnityTapScreen : AltBaseCommand
{
    float x;
    float y;
    public AltUnityTapScreen(SocketSettings socketSettings, float x, float y) : base(socketSettings)
    {
        this.x = x;
        this.y = y;
    }
    public AltUnityObject Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("tapScreen", x.ToString(), y.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
        if (data.Contains("error:notFound")) return null;
        HandleErrors(data);
        return null;
    }
}