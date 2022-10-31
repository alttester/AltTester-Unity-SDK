using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltDriver.Commands
{
    public class AltSetStaticProperty<T> : AltBaseCommand
    {
        AltSetObjectComponentPropertyParams cmdParams;
        public AltSetStaticProperty(IDriverCommunication commHandler, string componentName, string propertyName, string assemblyName, string newValue) : base(commHandler)
        {
            cmdParams = new AltSetObjectComponentPropertyParams(null, componentName, propertyName, assemblyName, newValue);
        }
        public T Execute()
        {
            CommHandler.Send(cmdParams);
            T data = CommHandler.Recvall<T>(cmdParams);
            return data;
        }
    }
}