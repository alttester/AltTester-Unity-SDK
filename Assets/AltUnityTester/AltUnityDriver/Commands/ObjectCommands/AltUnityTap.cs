public class AltUnityTap : AltUnityCommandReturningAltElement
{
    AltUnityObject altUnityObject;
    int count;
    
    public AltUnityTap(SocketSettings socketSettings, AltUnityObject altUnityObject, int count) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
        this.count = count;
    }
    
    public AltUnityObject Execute()
    {
        var altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("tapObject", altObject, count.ToString())));
        return ReceiveAltUnityObject();
    }
}