using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllComponents : AltBaseCommand
    {
        readonly AltUnityObject altUnityObject;

        public AltUnityGetAllComponents(SocketSettings socketSettings, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.altUnityObject = altUnityObject;
        }
        public List<AltUnityComponent> Execute()
        {
            SendCommand("getAllComponents", altUnityObject.id.ToString());
            string data = Recvall();
            return JsonConvert.DeserializeObject<List<AltUnityComponent>>(data);

        }
    }
}