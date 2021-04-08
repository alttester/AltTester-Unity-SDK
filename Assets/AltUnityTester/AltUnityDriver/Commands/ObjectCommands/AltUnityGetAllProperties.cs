using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllProperties : AltBaseCommand
    {
        AltUnityComponent altUnityComponent;
        readonly AltUnityObject altUnityObject;
        readonly AltUnityPropertiesSelections altUnityPropertiesSelections;
        public AltUnityGetAllProperties(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityPropertiesSelections altUnityPropertiesSelections = AltUnityPropertiesSelections.ALLPROPERTIES) : base(socketSettings)
        {
            this.altUnityComponent = altUnityComponent;
            this.altUnityObject = altUnityObject;
            this.altUnityPropertiesSelections = altUnityPropertiesSelections;
        }
        public List<AltUnityProperty> Execute()
        {
            var altComponent = JsonConvert.SerializeObject(altUnityComponent);
            SendCommand("getAllProperties", altUnityObject.id.ToString(), altComponent, altUnityPropertiesSelections.ToString());
            string data = Recvall();
            return JsonConvert.DeserializeObject<List<AltUnityProperty>>(data);
        }
    }
}