using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnitySwipe : AltBaseCommand
{
    AltUnityVector2 start;
    AltUnityVector2 end;
    float duration;
    public AltUnitySwipe(SocketSettings socketSettings, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(socketSettings)
    {
        this.start = start;
        this.end = end;
        this.duration = duration;
    }
    public void Execute()
    {
        var vectorStartJson = PositionToJson(start);
        var vectorEndJson = PositionToJson(end);
        
        Socket.Client.Send(toBytes(CreateCommand("MultipointSwipe", vectorStartJson, vectorEndJson, duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}