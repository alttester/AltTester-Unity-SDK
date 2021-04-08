using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllElementsLight : AltUnityBaseFindObjects
    {
        readonly By cameraBy;
        string cameraPath;
        readonly bool enabled;
        public AltUnityGetAllElementsLight(SocketSettings socketSettings, By cameraBy, string cameraPath, bool enabled) : base(socketSettings)
        {
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            this.enabled = enabled;
        }
        public List<AltUnityObjectLight> Execute()
        {
            cameraPath = SetPath(cameraBy, cameraPath);
            SendCommand("findObjectsLight", "//*", cameraBy.ToString(), cameraPath, enabled.ToString());

            string data = Recvall();
            var altElements = JsonConvert.DeserializeObject<List<AltUnityObjectLight>>(data);
            return altElements;

        }
    }
}