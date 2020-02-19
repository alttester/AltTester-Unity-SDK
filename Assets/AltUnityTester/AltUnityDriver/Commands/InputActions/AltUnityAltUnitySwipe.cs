using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityAltUnitySwipe : AltBaseCommand
{
    AltUnityVector2 start;
    AltUnityVector2 end;
    float duration;
    public AltUnityAltUnitySwipe(SocketSettings socketSettings, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(socketSettings)
    {
        this.start = start;
        this.end = end;
        this.duration = duration;
    }
    public void Execute(){
        string vectorStartJson = Newtonsoft.Json.JsonConvert.SerializeObject(start, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        string vectorEndJson = Newtonsoft.Json.JsonConvert.SerializeObject(end, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes(CreateCommand("MultipointSwipe", vectorStartJson, vectorEndJson, duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}