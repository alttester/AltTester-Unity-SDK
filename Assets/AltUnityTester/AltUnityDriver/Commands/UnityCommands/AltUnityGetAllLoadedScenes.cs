using System.Collections.Generic;

namespace Altom.AltUnityDriver.Commands
{
    internal class AltUnityGetAllLoadedScenes : AltBaseCommand
    {
        private readonly AltUnityGetAllLoadedScenesParams cmdParams;
        public AltUnityGetAllLoadedScenes(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllLoadedScenesParams();
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}