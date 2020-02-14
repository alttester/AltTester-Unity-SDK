public class AltUnityFindObject : AltUnityBaseFindObjects
{
    By by;
    string value;
    string cameraName;
    bool enabled;

    public AltUnityFindObject(SocketSettings socketSettings, By by, string value, string cameraName, bool enabled) : base(socketSettings)
    {
        this.by = by;
        this.value = value;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute(){
        if (enabled && by == By.NAME)
        {
            Socket.Client.Send(toBytes(CreateCommand("findActiveObjectByName", value, cameraName, enabled.ToString())));
        }
        else
        {
            string path = SetPath(by, value);
            Socket.Client.Send(toBytes(CreateCommand("findObject", path, cameraName, enabled.ToString())));
        }
        return ReceiveAltUnityObject();
    }
}