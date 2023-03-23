using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltBeginTouchCommand : AltCommand<AltBeginTouchParams, int>
    {
        public AltBeginTouchCommand(AltBeginTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override int Execute()
        {
            return InputController.BeginTouch(CommandParams.coordinates.ToUnity());
        }
    }
}
