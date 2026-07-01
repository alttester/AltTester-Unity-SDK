/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;

namespace AltTester.AltTesterSDK.Driver.Commands
{
    public class AltResetInput : AltBaseCommand
    {

        public AltResetInput(IDriverCommunication communicationHandler) : base(communicationHandler) { }
        public void Execute()
        {
            var cmdParams = new AltResetInputParams();
            this.CommHandler.Send(cmdParams);
            var data = this.CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}
