public class AltUnityGetAllCameras : AltBaseCommand
{
    public AltUnityGetAllCameras(SocketSettings socketSettings) : base(socketSettings)
    {
    }
    public System.Collections.Generic.List<string> Execute(){
        Socket.Client.Send(toBytes(CreateCommand("getAllCameras")));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<string>>(data);
        HandleErrors(data);
        return null;
    }
}