/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltGetApplicationScreenSize : AltBaseCommand
    {
        private readonly AltGetApplicationScreenSizeParams cmdParams;

        public AltGetApplicationScreenSize(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltGetApplicationScreenSizeParams();
        }

        public AltVector2 Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<AltVector2>(cmdParams);
        }
    }
}
