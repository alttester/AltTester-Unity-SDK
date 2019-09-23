public class PointerExitObject : CommandReturningAltElement
{
    AltUnityObject altUnityObject;

    public PointerExitObject(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute()
    {
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("pointerExitObject", altObject)));
        return ReceiveAltUnityObject();
    }
}