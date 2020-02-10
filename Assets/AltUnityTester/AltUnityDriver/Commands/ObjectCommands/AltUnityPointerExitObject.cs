public class AltUnityPointerExitObject : AltUnityCommandReturningAltElement
{
    AltUnityObject altUnityObject;

    public AltUnityPointerExitObject(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
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