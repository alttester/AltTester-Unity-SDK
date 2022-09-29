using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltDriver.Commands
{
    public class AltGetAllMethods : AltBaseCommand
    {
        AltGetAllMethodsParams cmdParams;
        public AltGetAllMethods(IDriverCommunication commHandler, AltComponent altUnityComponent, AltMethodSelection methodSelection = AltMethodSelection.ALLMETHODS) : base(commHandler)
        {
            cmdParams = new AltGetAllMethodsParams(altUnityComponent, methodSelection);
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}