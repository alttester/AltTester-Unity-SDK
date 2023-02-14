using AltTester.AltDriver;
using AltTester.AltDriver.Commands;

namespace AltTester.Commands
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