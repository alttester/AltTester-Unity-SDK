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
    public void Execute()
    {
        var locationJson = PositionToJson(location);
        Socket.Client.Send(toBytes(CreateCommand("moveMouse", locationJson, duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}