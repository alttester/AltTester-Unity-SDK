public class AltUnityGetAllMethods : AltBaseCommand
{
    AltUnityComponent altUnityComponent;
    AltUnityObject altUnityObject;
    AltUnityMethodSelection methodSelection;

    public AltUnityGetAllMethods(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityMethodSelection methodSelection=AltUnityMethodSelection.ALLMETHODS) : base(socketSettings)
    {
        this.altUnityComponent = altUnityComponent;
        this.altUnityObject = altUnityObject;
        this.methodSelection = methodSelection;
    }
    public System.Collections.Generic.List<string> Execute()
    {
        var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("getAllMethods", altComponent,methodSelection.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<string>>(data);
        HandleErrors(data);
        return null;
    }


}