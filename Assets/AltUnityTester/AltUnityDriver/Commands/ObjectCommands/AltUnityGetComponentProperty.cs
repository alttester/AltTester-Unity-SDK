namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetComponentProperty : AltBaseCommand
    {
        readonly string componentName;
        readonly string propertyName;
        readonly string assemblyName;
        readonly AltUnityObject altUnityObject;
        readonly int maxDepth;
        public AltUnityGetComponentProperty(SocketSettings socketSettings, string componentName, string propertyName, string assemblyName, int maxDepth, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.componentName = componentName;
            this.propertyName = propertyName;
            this.assemblyName = assemblyName;
            this.altUnityObject = altUnityObject;
            this.maxDepth = maxDepth;
        }
        public string Execute()
        {
            string altObject = Newtonsoft.Json.JsonConvert.SerializeObject(altUnityObject);
            string propertyInfo = Newtonsoft.Json.JsonConvert.SerializeObject(new AltUnityObjectProperty(componentName, propertyName, assemblyName));
            SendCommand("getObjectComponentProperty", altObject, propertyInfo, maxDepth.ToString());
            string data = Recvall();
            return data;
        }
    }
}