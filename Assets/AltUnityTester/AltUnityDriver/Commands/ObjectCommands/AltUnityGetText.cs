namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetText : AltBaseCommand
    {
        AltUnityObject altUnityObject;

        public AltUnityGetText(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
        }

        public string Execute()
        {
            string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            SendCommand("getText", altObject);
            string data = Recvall();
            if (!data.Contains("error:")) return data;
            HandleErrors(data);
            return null;
        }
    }
}