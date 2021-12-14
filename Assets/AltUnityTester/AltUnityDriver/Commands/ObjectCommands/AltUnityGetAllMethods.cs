using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllMethods : AltBaseCommand
    {
        AltUnityGetAllMethodsParams cmdParams;
        public AltUnityGetAllMethods(IDriverCommunication commHandler, AltUnityComponent altUnityComponent, AltUnityMethodSelection methodSelection = AltUnityMethodSelection.ALLMETHODS) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllMethodsParams(altUnityComponent, methodSelection);
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}