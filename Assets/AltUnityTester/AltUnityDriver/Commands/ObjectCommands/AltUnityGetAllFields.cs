using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllFields : AltBaseCommand
    {
        AltUnityComponent altUnityComponent;
        readonly AltUnityObject altUnityObject;
        readonly AltUnityFieldsSelections altUnityFieldsSelections;
        public AltUnityGetAllFields(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS) : base(socketSettings)
        {
            this.altUnityComponent = altUnityComponent;
            this.altUnityObject = altUnityObject;
            this.altUnityFieldsSelections = altUnityFieldsSelections;
        }
        public List<AltUnityProperty> Execute()
        {
            var altComponent = JsonConvert.SerializeObject(altUnityComponent);
            SendCommand("getAllFields", altUnityObject.id.ToString(), altComponent, altUnityFieldsSelections.ToString());
            string data = Recvall();
            return JsonConvert.DeserializeObject<List<AltUnityProperty>>(data);
        }
    }
}