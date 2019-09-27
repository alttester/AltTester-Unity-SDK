public class FindElements : CommandReturningAltElement
{
    string name;
    string cameraName;
    bool enabled;
    public FindElements(SocketSettings socketSettings, string name, string cameraName, bool enabled) : base(socketSettings)
    {
        this.name = name;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public System.Collections.Generic.List<AltUnityObject> Execute(){
        Socket.Client.Send(toBytes(CreateCommand("findObjectsByName", name, cameraName, enabled.ToString())));
        return ReceiveListOfAltUnityObjects();
    }
}