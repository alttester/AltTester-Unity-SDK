public class AltUnityFindObjectWhichContains : AltUnityBaseFindObjects
{
    By by;
    string value;
    string cameraName;
    bool enabled;

    public AltUnityFindObjectWhichContains(SocketSettings socketSettings, By by, string value, string cameraName, bool enabled) : base(socketSettings)
    {
        this.by = by;
        this.value = value;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute()
    {
        string path = SetPathContains(by, value);
        Socket.Client.Send(toBytes(CreateCommand("findObject", path, cameraName, enabled.ToString())));
        return ReceiveAltUnityObject();
    }
}