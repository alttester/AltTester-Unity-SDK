using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityTilt : AltBaseCommand
{
    AltUnityVector3 acceleration;
    float duration;
    public AltUnityTilt(SocketSettings socketSettings, AltUnityVector3 acceleration,float duration) : base(socketSettings)
    {
        this.acceleration = acceleration;
        this.duration = duration;
    }
    public void Execute()
    {
        string accelerationString = Newtonsoft.Json.JsonConvert.SerializeObject(acceleration, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes(CreateCommand("tilt", accelerationString,duration.ToString())));
        string data = Recvall();
        if (data.Equals("OK")) return;
        HandleErrors(data);
    }
}