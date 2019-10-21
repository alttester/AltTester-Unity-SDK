public class SwipeAndWait : AltBaseCommand
{
    System.Numerics.Vector2 start;
    System.Numerics.Vector2 end;
    float duration;
    public SwipeAndWait(SocketSettings socketSettings, System.Numerics.Vector2 start, System.Numerics.Vector2 end, float duration) : base(socketSettings)
    {
        this.start = start;
        this.end = end;
        this.duration = duration;
    }
    public void Execute()
    {
        new Swipe(SocketSettings, start, end, duration).Execute();
        System.Threading.Thread.Sleep((int)duration * 1000);
        string data;
        do
        {
            Socket.Client.Send(toBytes(CreateCommand("actionFinished")));
            data = Recvall();
        } while (data == "No");
        if (data.Equals("Yes"))
            return;
        HandleErrors(data);
    }
}