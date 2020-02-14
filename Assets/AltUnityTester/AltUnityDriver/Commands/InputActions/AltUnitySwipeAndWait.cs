using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnitySwipeAndWait : AltBaseCommand
{
    AltUnityVector2 start;
    AltUnityVector2 end;
    float duration;
    public AltUnitySwipeAndWait(SocketSettings socketSettings, AltUnityVector2 start, AltUnityVector2 end, float duration) : base(socketSettings)
    {
        this.start = start;
        this.end = end;
        this.duration = duration;
    }
    public void Execute()
    {
        new AltUnityAltUnitySwipe(SocketSettings, start, end, duration).Execute();
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