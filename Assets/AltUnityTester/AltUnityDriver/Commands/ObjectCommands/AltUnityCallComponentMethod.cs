using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallComponentMethod : AltBaseCommand
    {
        readonly string componentName;
        readonly string methodName;
        readonly string parameters;
        readonly string typeOfParameters;
        readonly string assemblyName;
        readonly AltUnityObject altUnityObject;
        public AltUnityCallComponentMethod(SocketSettings socketSettings, string componentName, string methodName, string parameters, string typeOfParameters, string assembly, AltUnityObject altUnityObject) : base(socketSettings)
        {
            this.componentName = componentName;
            this.methodName = methodName;
            this.parameters = parameters;
            this.typeOfParameters = typeOfParameters;
            this.assemblyName = assembly;
            this.altUnityObject = altUnityObject;
        }
        public string Execute()
        {
            string altObject = JsonConvert.SerializeObject(altUnityObject);
            string actionInfo =
                JsonConvert.SerializeObject(new AltUnityObjectAction(componentName, methodName, parameters, typeOfParameters, assemblyName));
            SendCommand("callComponentMethodForObject", altObject, actionInfo);
            string data = Recvall();
            return data;
        }
    }
}