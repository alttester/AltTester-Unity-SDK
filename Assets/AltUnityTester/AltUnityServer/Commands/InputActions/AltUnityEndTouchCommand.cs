using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityEndTouchCommand : AltUnityCommand<AltUnityEndTouchParams, string>
    {
        int fingerId;
        public AltUnityEndTouchCommand(AltUnityEndTouchParams cmdParams) : base(cmdParams)
        {
        }
        public override string Execute()
        {
            InputController.EndTouch(CommandParams.fingerId);
            return "Ok";
        }
    }
}