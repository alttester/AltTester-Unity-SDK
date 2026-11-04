/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;
using Newtonsoft.Json;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetAllMethods : AltBaseCommand
    {
        AltGetAllMethodsParams cmdParams;
        public AltGetAllMethods(IDriverCommunication commHandler, AltComponent altComponent, AltMethodSelection methodSelection = AltMethodSelection.ALLMETHODS) : base(commHandler)
        {
            cmdParams = new AltGetAllMethodsParams(altComponent, methodSelection);
        }
        public List<string> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<List<string>>(cmdParams);
        }
    }
}
