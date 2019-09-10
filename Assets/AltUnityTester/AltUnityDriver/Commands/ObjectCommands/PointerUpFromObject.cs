public class PointerUpFromObjectDriver : AltBaseCommand
{
    AltUnityObject altUnityObject;

    public PointerUpFromObjectDriver(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
    {
        this.altUnityObject = altUnityObject;
    }
    public AltUnityObject Execute(){
        string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
        Socket.Client.Send( System.Text.Encoding.ASCII.GetBytes(CreateCommand("pointerUpFromObject", altObject )));
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            return altElement;
        }

        HandleErrors(data);
        return null;
    }
}