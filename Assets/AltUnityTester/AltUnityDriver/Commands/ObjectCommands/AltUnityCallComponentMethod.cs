using System.Linq;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityCallComponentMethod<T> : AltBaseCommand
    {
        AltUnityCallComponentMethodForObjectParams cmdParams;

        public AltUnityCallComponentMethod(IDriverCommunication commHandler, string componentName, string methodName, object[] parameters, string[] typeOfParameters, string assembly, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityCallComponentMethodForObjectParams(altUnityObject, componentName, methodName, parameters.Select(p => JsonConvert.SerializeObject(p, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = System.Globalization.CultureInfo.InvariantCulture
            })).ToArray(), typeOfParameters, assembly);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T data = CommHandler.Recvall<T>(cmdParams);
            return data;
        }
    }
}