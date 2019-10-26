using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class Swipe : AltBaseCommand
{
    Vector2 start;
    Vector2 end;
    float duration;
    public Swipe(SocketSettings socketSettings, Vector2 start, Vector2 end, float duration) : base(socketSettings)
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
        Socket.Client.Send(toBytes(CreateCommand("movingTouch", vectorStartJson, vectorEndJson, duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}