using System.Collections.Generic;
using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetAllScenes : AltBaseCommand
    {
        private readonly AltUnityGetAllScenesParams cmdParams;
        public AltUnityGetAllScenes(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltUnityGetAllScenesParams();
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}