namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllProperties : AltBaseCommand
    {
        AltUnityComponent altUnityComponent;
        AltUnityObject altUnityObject;
        AltUnityPropertiesSelections altUnityPropertiesSelections;
        public AltUnityGetAllProperties(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityPropertiesSelections altUnityPropertiesSelections = AltUnityPropertiesSelections.ALLPROPERTIES) : base(socketSettings)
        {
            this.altUnityComponent = altUnityComponent;
            this.altUnityObject = altUnityObject;
            this.altUnityPropertiesSelections = altUnityPropertiesSelections;
        }
        public System.Collections.Generic.List<AltUnityProperty> Execute()
        {
            var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
            SendCommand("getAllProperties", altUnityObject.id.ToString(), altComponent, altUnityPropertiesSelections.ToString());
            string data = Recvall();
            if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityProperty>>(data);
            HandleErrors(data);
            return null;
        }
    }
}