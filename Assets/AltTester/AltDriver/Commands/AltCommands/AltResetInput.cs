using AltTester.AltDriver.Commands;

namespace Altom.AltDriver.Commands
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