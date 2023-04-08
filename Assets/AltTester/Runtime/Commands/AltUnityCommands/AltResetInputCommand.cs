using AltTester.AltTesterUnitySDK;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Commands;

namespace Altom.AltTester.AltTesterUnitySDK.Commands
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
