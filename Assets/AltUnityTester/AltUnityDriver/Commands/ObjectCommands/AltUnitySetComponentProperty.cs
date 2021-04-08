namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetComponentProperty : AltBaseCommand
    {
        readonly string componentName;
        readonly string propertyName;
        readonly string value;
        readonly string assemblyName;
        readonly AltUnityObject altUnityObject;

        public AltUnitySetComponentProperty(SocketSettings socketSettings, string componentName, string propertyName, string value, string assemblyName, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.componentName = componentName;
            this.propertyName = propertyName;
            this.value = value;
            this.assemblyName = assemblyName;
            this.altUnityObject = altUnityObject;
        }
        public string Execute()
        {
            string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            string propertyInfo = Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectProperty(componentName, propertyName, assemblyName));
            SendCommand("setObjectComponentProperty", altObject, propertyInfo, value);
            string data = Recvall();
            return data;
        }
    }
}