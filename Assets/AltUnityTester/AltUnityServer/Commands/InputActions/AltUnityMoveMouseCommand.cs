using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityMoveMouseCommand : AltUnityCommandWithWait<AltUnityMoveMouseParams, string>
    {
        public AltUnityMoveMouseCommand(ICommandHandler handler, AltUnityMoveMouseParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.MoveMouse(CommandParams.coordinates.ToUnity(), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
