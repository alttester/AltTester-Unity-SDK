public class TiltDriver : AltBaseCommand
{
    UnityEngine.Vector3 acceleration;
    public TiltDriver(SocketSettings socketSettings, UnityEngine.Vector3 acceleration) : base(socketSettings)
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