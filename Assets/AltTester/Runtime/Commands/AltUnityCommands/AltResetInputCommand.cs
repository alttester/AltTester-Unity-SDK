using AltTester.AltTesterUnitySdk;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Commands;

namespace Altom.AltTester.AltTesterUnitySdk.Commands
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
