public class ClickEventDriver : CommandReturningAltElement
{
    AltUnityObject altUnityObject;
    public ClickEventDriver(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("clickEvent", altObject)));
        return ReceiveAltUnityObject();
    }
}