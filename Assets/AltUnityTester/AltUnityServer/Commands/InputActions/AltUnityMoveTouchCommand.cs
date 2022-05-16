using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityMoveTouchCommand : AltUnityCommand<AltUnityMoveTouchParams, string>
    {
        public AltUnityMoveTouchCommand(AltUnityMoveTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override string Execute()
        {
            InputController.MoveTouch(CommandParams.fingerId, CommandParams.coordinates.ToUnity());
            return "Ok";

        }
    }
}