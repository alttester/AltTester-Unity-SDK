using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallStaticMethod<T> : AltBaseCommand
    {
        AltUnityCallComponentMethodForObjectParams cmdParams;

        public AltUnityCallStaticMethod(IDriverCommunication commHandler, string typeName, string methodName, string[] parameters, string[] typeOfParameters, string assemblyName) : base(commHandler)
        {
            cmdParams = new AltUnityCallComponentMethodForObjectParams(null, typeName, methodName, parameters, typeOfParameters, assemblyName);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<T>(cmdParams).data;
        }
    }
}