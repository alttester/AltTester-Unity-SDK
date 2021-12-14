using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllComponents : AltBaseCommand
    {
        AltUnityGetAllComponentsParams cmdParams;
        public AltUnityGetAllComponents(IDriverCommunication commHandler, AltUnityObject altUnityObject) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllComponentsParams(altUnityObject.id);
        }
        public List<AltUnityComponent> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<AltUnityComponent>>(cmdParams);

        }
    }
}