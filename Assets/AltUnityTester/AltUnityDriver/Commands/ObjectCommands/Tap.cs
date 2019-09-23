public class Tap : CommandReturningAltElement
{
    AltUnityObject altUnityObject;
    public Tap(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("tapObject", altObject)));
        return ReceiveAltUnityObject();
    }
}