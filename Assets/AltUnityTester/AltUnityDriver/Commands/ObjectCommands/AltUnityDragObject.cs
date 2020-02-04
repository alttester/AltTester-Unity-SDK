using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

public class AltUnityDragObject : AltUnityCommandReturningAltElement
{
    AltUnityVector2 position;
    AltUnityObject altUnityObject;
    public AltUnityDragObject(SocketSettings socketSettings, AltUnityVector2 position, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.position = position;
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute(){
        string positionString = Newtonsoft.Json.JsonConvert.SerializeObject(position, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        });
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(CreateCommand("dragObject",positionString , altObject )));
        return ReceiveAltUnityObject();
    }
}