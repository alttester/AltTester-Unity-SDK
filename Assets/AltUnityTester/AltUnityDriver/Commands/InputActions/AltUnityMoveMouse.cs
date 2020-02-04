using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityMoveMouse : AltBaseCommand
{
    AltUnityVector2 location;
    float duration;
    public AltUnityMoveMouse(SocketSettings socketSettings, AltUnityVector2 location, float duration) : base(socketSettings)
    {
        this.location = location;
        this.duration = duration;
    }
    public void Execute(){
        string locationJson = Newtonsoft.Json.JsonConvert.SerializeObject(location, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes(CreateCommand("moveMouse", locationJson.ToString(), duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}