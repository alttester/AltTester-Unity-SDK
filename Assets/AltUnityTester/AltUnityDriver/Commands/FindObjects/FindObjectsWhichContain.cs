public class FindObjectsWhichContain : BaseFindObjects
{
    By by;
    string value;
    string cameraName;
    bool enabled;

    public FindObjectsWhichContain(SocketSettings socketSettings, By by, string value, string cameraName, bool enabled) : base(socketSettings)
    {
        this.by = by;
        this.value = value;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute()
    {
        string path = SetPathContains(by, value);
        Socket.Client.Send(toBytes(CreateCommand("findObjects", path, cameraName, enabled.ToString())));
        return ReceiveListOfAltUnityObjects();
    }
}