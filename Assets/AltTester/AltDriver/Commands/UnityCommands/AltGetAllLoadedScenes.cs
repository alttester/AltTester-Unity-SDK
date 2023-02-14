using System.Collections.Generic;

namespace AltTester.AltDriver.Commands
{
    internal class AltGetAllLoadedScenes : AltBaseCommand
    {
        private readonly AltGetAllLoadedScenesParams cmdParams;
        public AltGetAllLoadedScenes(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltGetAllLoadedScenesParams();
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}