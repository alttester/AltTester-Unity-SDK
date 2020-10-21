
public class AltUnityGetAllFields : AltBaseCommand
{
    AltUnityComponent altUnityComponent;
    AltUnityObject altUnityObject;
    AltUnityFieldsSelections altUnityFieldsSelections;
    public AltUnityGetAllFields(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS) : base(socketSettings)
    {
        this.altUnityComponent = altUnityComponent;
        this.altUnityObject = altUnityObject;
        this.altUnityFieldsSelections = altUnityFieldsSelections;
    }
    public System.Collections.Generic.List<AltUnityField> Execute()
    {
        var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
        Socket.Client.Send(System.Text.Encoding.ASCII.GetBytes(CreateCommand("getAllFields", altUnityObject.id.ToString(), altComponent, altUnityFieldsSelections.ToString())));
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityField>>(data);
        HandleErrors(data);
        return null;
    }
}