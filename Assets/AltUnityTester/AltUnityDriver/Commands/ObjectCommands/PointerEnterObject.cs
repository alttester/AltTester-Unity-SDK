public class PointerEnterObject : CommandReturningAltElement
{
    AltUnityObject altUnityObject;
    public PointerEnterObject(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("pointerEnterObject", altObject)));
        return ReceiveAltUnityObject();
    }
}