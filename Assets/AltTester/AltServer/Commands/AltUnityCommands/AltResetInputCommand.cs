using AltTester;
using AltTester.AltDriver.Commands;
using AltTester.Commands;

namespace Altom.AltTester.Commands
{
    public class AltResetInputCommand : AltCommand<AltResetInputParams, string>
    {
        public AltResetInputCommand(AltResetInputParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            InputController.ResetInput();
            return "Ok";
        }
    }
}
