public class AltUnityGetAllComponents : AltBaseCommand
{
    AltUnityObject AltUnityObject;

    public AltUnityGetAllComponents(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
    {
        AltUnityObject = altUnityObject;
    }
    public System.Collections.Generic.List<AltUnityComponent> Execute()
    {
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("getAllComponents", AltUnityObject.id.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityComponent>>(data);
        HandleErrors(data);
        return null;
    }
}