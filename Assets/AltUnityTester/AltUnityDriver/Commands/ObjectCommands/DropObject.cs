public class DropObjectDriver : AltBaseCommand
{
    UnityEngine.Vector2 position;
    AltUnityObject altUnityObject;

    public DropObjectDriver(SocketSettings socketSettings, UnityEngine.Vector2 position, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.position = position;
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        string positionString = Newtonsoft.Json.JsonConvert.SerializeObject(position, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("dropObject", positionString, altObject)));
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            return altElement;
        }

        HandleErrors(data);
        return null;
    }
}