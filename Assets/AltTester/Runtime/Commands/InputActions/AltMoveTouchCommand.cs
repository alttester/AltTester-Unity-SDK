using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    public class AltMoveTouchCommand : AltCommand<AltMoveTouchParams, string>
    {
        public AltMoveTouchCommand(AltMoveTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override string Execute()
        {
            InputController.MoveTouch(CommandParams.fingerId, CommandParams.coordinates.ToUnity());
            return "Ok";

        }
    }
}