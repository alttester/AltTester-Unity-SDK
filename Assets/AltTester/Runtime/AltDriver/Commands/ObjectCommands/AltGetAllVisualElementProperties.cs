/*
    Copyright(C) 2026 Altom Consulting
*/

using System.Collections.Generic;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetAllVisualElementProperties : AltBaseCommand
    {
        AltGetAllVisualElementPropertyParams cmdParams;
        public AltGetAllVisualElementProperties(IDriverCommunication commHandler, AltObject altObject) : base(commHandler)
        {
            cmdParams = new AltGetAllVisualElementPropertyParams(altObject);
        }
        public Dictionary<string, object> Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<Dictionary<string, object>>(cmdParams);
        }
    }
}
