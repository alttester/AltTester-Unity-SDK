public class AltUnityCommandReturningAltElement : AltBaseCommand
{
    public AltUnityCommandReturningAltElement(SocketSettings socketSettings) : base(socketSettings)
    {
    }

    protected AltUnityObject ReceiveAltUnityObject()
    {
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            AltUnityObject altElement = Newtonsoft.Json.JsonConvert.DeserializeObject<AltUnityObject>(data);
            altElement.socketSettings = SocketSettings;
            return altElement;
        }
        HandleErrors(data);
        return null;
    }
    protected System.Collections.Generic.List<AltUnityObject> ReceiveListOfAltUnityObjects()
    {
        string data = Recvall();
        if (!data.Contains("error:"))
        {
            var altElements= Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObject>>(data);
            foreach(var altElement in altElements)
            {
                altElement.socketSettings = SocketSettings;
            }
            return altElements;
        }
        HandleErrors(data);
        return null;
    }

}