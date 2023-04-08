using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltMoveMouseCommand : AltCommandWithWait<AltMoveMouseParams, string>
    {
        public AltMoveMouseCommand(ICommandHandler handler, AltMoveMouseParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.MoveMouse(CommandParams.coordinates.ToUnity(), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
