public class AltUnityFindObjectsWhichContain : AltUnityBaseFindObjects
{
    By by;
    string value;
    string cameraName;
    bool enabled;

    public AltUnityFindObjectsWhichContain(SocketSettings socketSettings, By by, string value, string cameraName, bool enabled) : base(socketSettings)
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