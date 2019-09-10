public class PointerEnterObjectDriver : CommandReturningAltElement
{
    AltUnityObject altUnityObject;
    public PointerEnterObjectDriver(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
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