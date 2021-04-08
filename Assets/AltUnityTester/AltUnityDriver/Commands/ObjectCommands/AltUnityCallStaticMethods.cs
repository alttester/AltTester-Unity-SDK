using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallStaticMethod : AltBaseCommand
    {
        readonly string typeName;
        readonly string methodName;
        readonly string parameters;
        readonly string typeOfParameters;
        readonly string assemblyName;
        public AltUnityCallStaticMethod(SocketSettings socketSettings, string typeName, string methodName, string parameters, string typeOfParameters, string assemblyName) : base(socketSettings)
        {
            this.typeName = typeName;
            this.methodName = methodName;
            this.parameters = parameters;
            this.typeOfParameters = typeOfParameters;
            this.assemblyName = assemblyName;
        }
        public string Execute()
        {
            string actionInfo = JsonConvert.SerializeObject(new AltUnityObjectAction(typeName, methodName, parameters, typeOfParameters, assemblyName));
            SendCommand("callComponentMethodForObject", "", actionInfo);
            var data = Recvall();
            return data;
        }
    }
}