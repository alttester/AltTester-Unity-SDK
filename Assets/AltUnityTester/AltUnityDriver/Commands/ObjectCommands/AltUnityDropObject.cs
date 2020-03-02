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
        var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        var positionString = PositionToJson(position);
        
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("dropObject", positionString, altObject)));
        return ReceiveAltUnityObject();
    }
}