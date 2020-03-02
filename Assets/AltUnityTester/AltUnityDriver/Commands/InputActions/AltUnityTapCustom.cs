public class AltUnityTapCustom : AltBaseCommand
{
    float x;
    float y;
    int count;
    float interval;
    public AltUnityTapCustom(SocketSettings socketSettings, float x, float y, int count, float interval) : base(socketSettings)
    {
        this.x = x;
        this.y = y;
        this.count = count;
        this.interval = interval;
    }
    public void Execute()
    {
        var posJson = PositionToJson(x, y);
        Socket.Client.Send(toBytes(CreateCommand("tapCustom", posJson, count.ToString(), interval.ToString())));
        string data = Recvall();
        if (data.Equals("OK")) return;
        HandleErrors(data);
    }
}