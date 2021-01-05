namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllFields : AltBaseCommand
    {
        AltUnityComponent altUnityComponent;
        AltUnityObject altUnityObject;
        AltUnityFieldsSelections altUnityFieldsSelections;
        public AltUnityGetAllFields(SocketSettings socketSettings, AltUnityComponent altUnityComponent, AltUnityObject altUnityObject, AltUnityFieldsSelections altUnityFieldsSelections = AltUnityFieldsSelections.ALLFIELDS) : base(socketSettings)
        {
            this.altUnityComponent = altUnityComponent;
            this.altUnityObject = altUnityObject;
            this.altUnityFieldsSelections = altUnityFieldsSelections;
        }
        public System.Collections.Generic.List<AltUnityProperty> Execute()
        {
            var altComponent = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityComponent);
            SendCommand("getAllFields", altUnityObject.id.ToString(), altComponent, altUnityFieldsSelections.ToString());
            string data = Recvall();
            if (!data.Contains("error:")) return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<AltUnityProperty>>(data);
            HandleErrors(data);
            return null;
        }
    }
}