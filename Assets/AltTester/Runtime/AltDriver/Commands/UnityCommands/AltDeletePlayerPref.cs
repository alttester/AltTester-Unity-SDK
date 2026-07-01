/*
    Copyright(C) 2026 Altom Consulting
*/

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltDeletePlayerPref : AltBaseCommand
    {
        AltDeletePlayerPrefParams cmdParams;
        public AltDeletePlayerPref(IDriverCommunication commHandler) : base(commHandler)
        {
            this.cmdParams = new AltDeletePlayerPrefParams();
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}
