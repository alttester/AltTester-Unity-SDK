public class AltUnityFindObject : AltUnityBaseFindObjects
{
    By by;
    string value;
    By cameraBy;
    string cameraPath;
    bool enabled;

    public AltUnityFindObject(SocketSettings socketSettings, By by, string value,  By cameraBy,string cameraPath, bool enabled) : base(socketSettings)
    {
        this.by = by;
        this.value = value;
        this.cameraBy = cameraBy;
        this.cameraPath = cameraPath;
        this.enabled = enabled;
    }
    public AltUnityObject Execute(){
        cameraPath = SetPath(cameraBy, cameraPath);
        if (enabled && by == By.NAME)
        {
            Socket.Client.Send(toBytes(CreateCommand("findActiveObjectByName", value, cameraBy.ToString(),cameraPath, enabled.ToString())));
        }
        else
        {
            string path = SetPath(by, value);
            Socket.Client.Send(toBytes(CreateCommand("findObject", path, cameraBy.ToString(), cameraPath, enabled.ToString())));
        }
        return ReceiveAltUnityObject();
    }
}