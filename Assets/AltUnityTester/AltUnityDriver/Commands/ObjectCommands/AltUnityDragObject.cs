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
    public AltUnityObject Execute()
    {
        var positionJson = PositionToJson(position);
        var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        
        Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(CreateCommand("dragObject", positionJson, altObject)));
        return ReceiveAltUnityObject();
    }
}