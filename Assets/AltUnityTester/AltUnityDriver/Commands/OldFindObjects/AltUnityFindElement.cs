public class AltUnityFindElement : AltUnityCommandReturningAltElement
{
    string name;
    string cameraName;
    bool enabled;
    public AltUnityFindElement(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute(){
        Socket.Client.Send(toBytes(CreateCommand("findObjectByName", name, cameraName, enabled.ToString())));
        return ReceiveAltUnityObject();
    }
}