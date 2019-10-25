public class SetText : CommandReturningAltElement
{
    AltUnityObject altUnityObject;
    string newText;

    public SetText(SocketSettings socketSettings, AltUnityObject altUnityObject, string text) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
        this.newText = text;
    }

    public AltUnityObject Execute()
    {
        var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("setText", altObject, newText)));
        return ReceiveAltUnityObject();
    }
}