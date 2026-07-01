/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Linq;
using Newtonsoft.Json;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltCallComponentMethod<T> : AltBaseCommand
    {
        AltCallComponentMethodForObjectParams cmdParams;

        public AltCallComponentMethod(IDriverCommunication commHandler, string componentName, string methodName, object[] parameters, string[] typeOfParameters, string assembly, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltCallComponentMethodForObjectParams(altObject, componentName, methodName, parameters.Select(p => JsonConvert.SerializeObject(p, JsonSerializerSettings)).ToArray(), typeOfParameters, assembly);
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
