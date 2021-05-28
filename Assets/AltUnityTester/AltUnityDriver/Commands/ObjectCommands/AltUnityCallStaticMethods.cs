using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallStaticMethod : AltBaseCommand
    {
        AltUnityCallComponentMethodForObjectParams cmdParams;

        public AltUnityCallStaticMethod(IDriverCommunication commHandler, string typeName, string methodName, string[] parameters, string[] typeOfParameters, string assemblyName) : base(commHandler)
        {
            cmdParams = new AltUnityCallComponentMethodForObjectParams(null, typeName, methodName, parameters, typeOfParameters, assemblyName);
        }
        public string Execute()
        {
            CommHandler.Send(cmdParams);
            string data = CommHandler.Recvall<string>(cmdParams).data;
            return data;
        }
    }
}