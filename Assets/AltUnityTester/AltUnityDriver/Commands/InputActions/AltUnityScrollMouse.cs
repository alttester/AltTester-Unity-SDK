public class AltUnityScrollMouse : AltBaseCommand
{
    float speed;
    float duration;
    public AltUnityScrollMouse(SocketSettings socketSettings, float speed, float duration) : base(socketSettings)
    {
        this.speed = speed;
        this.duration = duration;
    }
    public void Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("scrollMouse", speed.ToString(), duration.ToString())));
        var data = Recvall();
        if (data.Equals("Ok"))
            return;
        HandleErrors(data);
    }
}