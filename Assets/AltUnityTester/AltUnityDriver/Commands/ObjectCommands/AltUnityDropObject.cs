using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityDropObject : AltUnityCommandReturningAltElement
{
    AltUnityVector2 position;
    AltUnityObject altUnityObject;

    public AltUnityDropObject(SocketSettings socketSettings, AltUnityVector2 position, AltUnityObject altUnityObject) : base(socketSettings)
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
        return ReceiveAltUnityObject();

    }
}