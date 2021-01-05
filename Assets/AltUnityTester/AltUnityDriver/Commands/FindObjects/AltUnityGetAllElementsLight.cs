namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllElementsLight : AltUnityBaseFindObjects
    {
        By cameraBy;
        string cameraPath;
        bool enabled;
        public AltUnityGetAllElementsLight(SocketSettings socketSettings, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public System.Collections.Generic.List<AltUnityObjectLight> Execute()
        {
            cameraPath = SetPath(cameraBy, cameraPath);
            SendCommand("findObjectsLight", "//*", cameraBy.ToString(), cameraPath, enabled.ToString());
            string data = Recvall();
            if (!data.Contains("error:"))
            {
                var altElements = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityObjectLight>>(data);
                return altElements;
            }
            HandleErrors(data);
            return null;
        }
    }
}