/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;

namespace AltTester.AltTesterSDK.Driver.Commands
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
