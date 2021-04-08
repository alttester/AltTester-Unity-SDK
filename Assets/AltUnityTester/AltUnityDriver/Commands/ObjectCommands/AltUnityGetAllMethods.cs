using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllMethods : AltBaseCommand
    {
        AltUnityComponent altUnityComponent;
        readonly AltUnityObject altUnityObject;
        readonly AltUnityMethodSelection methodSelection;

        public AltUnityGetAllMethods(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityMethodSelection methodSelection = AltUnityMethodSelection.ALLMETHODS) : base(socketSettings)
        {
            this.altUnityComponent = altUnityComponent;
            this.altUnityObject = altUnityObject;
            this.methodSelection = methodSelection;
        }
        public List<string> Execute()
        {
            var altComponent = JsonConvert.SerializeObject(altUnityComponent);
            SendCommand("getAllMethods", altComponent, methodSelection.ToString());
            string data = Recvall();
            return JsonConvert.DeserializeObject<List<string>>(data);

        }
    }
}