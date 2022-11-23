using System.Collections.Generic;
using Newtonsoft.Json;

namespace AltTester.AltDriver.Commands
{
    public class AltGetAllScenes : AltBaseCommand
    {
        private readonly AltGetAllScenesParams cmdParams;
        public AltGetAllScenes(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltGetAllScenesParams();
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}