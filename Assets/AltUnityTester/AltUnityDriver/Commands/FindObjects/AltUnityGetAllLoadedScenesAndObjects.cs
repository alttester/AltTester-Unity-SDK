namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllLoadedScenesAndObjects : AltUnityBaseFindObjects
    {

        public AltUnityGetAllLoadedScenesAndObjects(SocketSettings socketSettings) : base(socketSettings)
        {

        }
        public System.Collections.Generic.List<AltUnityObjectLight> Execute()
        {
            SendCommand("getAllLoadedScenesAndObjects", "//*", "NAME", "", true.ToString());
            string data = Recvall();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObjectLight>>(data);
        }
    }
}