/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetServerVersion : AltBaseCommand
    {
        public AltGetServerVersion(IDriverCommunication commHandler) : base(commHandler)
        {
        }
        public string Execute()
        {
            var cmdParams = new AltGetServerVersionParams();
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams);
        }
    }
}
