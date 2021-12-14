using System.Linq;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallStaticMethod<T> : AltBaseCommand
    {
        AltUnityCallComponentMethodForObjectParams cmdParams;

        public AltUnityCallStaticMethod(IDriverCommunication commHandler, string typeName, string methodName, object[] parameters, string[] typeOfParameters, string assemblyName) : base(commHandler)
        {
            cmdParams = new AltUnityCallComponentMethodForObjectParams(null, typeName, methodName, parameters.Select(p => JsonConvert.SerializeObject(p, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = System.Globalization.CultureInfo.InvariantCulture
            })).ToArray(), typeOfParameters, assemblyName);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<T>(cmdParams);
        }
    }
}