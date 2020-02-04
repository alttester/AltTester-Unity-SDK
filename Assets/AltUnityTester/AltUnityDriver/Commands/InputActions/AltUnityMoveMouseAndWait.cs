using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityMoveMouseAndWait : AltBaseCommand
{
    AltUnityVector2 location;
    float duration;
    public AltUnityMoveMouseAndWait(SocketSettings socketSettings, AltUnityVector2 location, float duration) : base(socketSettings)
    {
        this.location = location;
        this.duration = duration;
    }
    public void Execute(){
        new AltUnityMoveMouse(SocketSettings,location, duration).Execute();
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