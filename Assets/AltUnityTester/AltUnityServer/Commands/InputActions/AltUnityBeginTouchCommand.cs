using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnityBeginTouchCommand : AltUnityCommand<AltUnityBeginTouchParams, int>
    {
        public AltUnityBeginTouchCommand(AltUnityBeginTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override int Execute()
        {
            return InputController.BeginTouch(CommandParams.coordinates.ToUnity());
        }
    }
}
