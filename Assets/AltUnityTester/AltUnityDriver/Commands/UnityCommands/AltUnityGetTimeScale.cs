public class AltUnityGetTimeScale : AltBaseCommand
{
    public AltUnityGetTimeScale(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public float Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("getTimeScale")));
        var data = Recvall();
        if (!data.Contains("error"))
            return Newtonsoft.Json.JsonConvert.DeserializeObject<float>(data);
        HandleErrors(data);
        return -1f;
    }
}