/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Linq;
using Newtonsoft.Json;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltCallStaticMethod<T> : AltBaseCommand
    {
        AltCallComponentMethodForObjectParams cmdParams;

        public AltCallStaticMethod(IDriverCommunication commHandler, string typeName, string methodName, object[] parameters, string[] typeOfParameters, string assemblyName) : base(commHandler)
        {
            cmdParams = new AltCallComponentMethodForObjectParams(null, typeName, methodName, parameters.Select(p => JsonConvert.SerializeObject(p, JsonSerializerSettings)).ToArray(), typeOfParameters, assemblyName);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T result = CommHandler.Recvall<T>(cmdParams);

            // If the result is an AltObject, ensure its CommHandler is set
            if (result is AltObject altObject)
            {
                altObject.CommHandler = CommHandler;
            }

            return result;
        }
    }
}
