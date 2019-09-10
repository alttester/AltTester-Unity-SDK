public class CommandReturningAltElement : AltBaseCommand
{
    public CommandReturningAltElement(SocketSettings socketSettings) : base(socketSettings)
    {
    }

    protected AltUnityObject ReceiveAltUnityObject()
    {
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            return altElement;
        }
        HandleErrors(data);
        return null;
    }
    protected System.Collections.Generic.List<AltUnityObject> ReceiveListOfAltUnityObjects()
    {
        string data = Recvall();
        if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
        HandleErrors(data);
        return null;
    }

}