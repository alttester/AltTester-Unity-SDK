public class AltUnityGetAllProperties : AltBaseCommand
{
    AltUnityComponent altUnityComponent;
    AltUnityObject altUnityObject;
    public AltUnityGetAllProperties(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.altUnityComponent = altUnityComponent;
        this.altUnityObject = altUnityObject;
    }
    public System.Collections.Generic.List<AltUnityProperty> Execute()
    {
        var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("getAllFields", altUnityObject.id.ToString(), altComponent)));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityProperty>>(data);
        HandleErrors(data);
        return null;
    }
}