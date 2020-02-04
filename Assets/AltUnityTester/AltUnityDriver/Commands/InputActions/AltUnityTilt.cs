using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityTilt : AltBaseCommand
{
    AltUnityVector3 acceleration;
    public AltUnityTilt(SocketSettings socketSettings, AltUnityVector3 acceleration) : base(socketSettings)
    {
        this.acceleration = acceleration;
    }
    public void Execute()
    {
        string accelerationString = Newtonsoft.Json.JsonConvert.SerializeObject(acceleration, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(toBytes(CreateCommand("tilt", accelerationString)));
        string data = Recvall();
        if (data.Equals("OK")) return;
        HandleErrors(data);
    }
}