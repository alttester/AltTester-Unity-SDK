namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllLoadedScenesAndObjects : AltUnityBaseFindObjects
    {

        private bool enabled;
        public AltUnityGetAllLoadedScenesAndObjects(SocketSettings socketSettings, bool enabled) : base(socketSettings)
        {
            this.enabled = enabled;
        }
        public System.Collections.Generic.List<AltUnityObjectLight> Execute()
        {
            SendCommand("getAllLoadedScenesAndObjects", "//*", "NAME", "", enabled.ToString());
            string data = Recvall();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObjectLight>>(data);
        }
    }
}