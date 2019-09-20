public class FindElementWhereNameContainsDriver : CommandReturningAltElement
{
    string name;
    string cameraName;
    bool enabled;
    public FindElementWhereNameContainsDriver(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
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