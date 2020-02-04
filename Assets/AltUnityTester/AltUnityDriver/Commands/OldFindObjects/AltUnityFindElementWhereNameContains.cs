public class AltUnityFindElementWhereNameContains : AltUnityCommandReturningAltElement
{
    string name;
    string cameraName;
    bool enabled;
    public AltUnityFindElementWhereNameContains(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute()
    {
        Socket.Client.Send(toBytes(CreateCommand("findObjectWhereNameContains", name, cameraName, enabled.ToString())));
        return ReceiveAltUnityObject();
    }
}