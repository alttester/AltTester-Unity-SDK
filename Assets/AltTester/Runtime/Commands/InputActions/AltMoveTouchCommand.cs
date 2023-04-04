using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;

namespace AltTester.AltTesterUnitySDK.Commands
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