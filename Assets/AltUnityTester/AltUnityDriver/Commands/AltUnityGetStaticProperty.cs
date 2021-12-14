using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetStaticProperty<T> : AltBaseCommand
    {
        AltUnityGetObjectComponentPropertyParams cmdParams;
        public AltUnityGetStaticProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, int maxDepth) : base(commHandler)
        {
            cmdParams = new AltUnityGetObjectComponentPropertyParams(null, componentName, propertyName, assemblyName, maxDepth);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T data = CommHandler.Recvall<T>(cmdParams);
            return data;
        }
    }
}