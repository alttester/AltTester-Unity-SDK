public class Swipe : AltBaseCommand
{
    UnityEngine.Vector2 start;
    UnityEngine.Vector2 end;
    float duration;
    public Swipe(SocketSettings socketSettings, UnityEngine.Vector2 start, UnityEngine.Vector2 end, float duration) : base(socketSettings)
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