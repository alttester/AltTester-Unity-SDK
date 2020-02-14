public class AltUnityFindElementByComponent : AltUnityCommandReturningAltElement
{
    string componentName;
    string assemblyName;
    string cameraName;
    bool enabled;
    public AltUnityFindElementByComponent(SocketSettings socketSettings, string componentName, string assemblyName, string cameraName, bool enabled) : base(socketSettings)
    {
        this.componentName = componentName;
        this.assemblyName = assemblyName;
        this.cameraName = cameraName;
        this.enabled = enabled;
    }
    public AltUnityObject Execute(){
        Socket.Client.Send(toBytes(CreateCommand("findObjectByComponent", assemblyName, componentName, cameraName, enabled.ToString())));
        return ReceiveAltUnityObject();
    }
}