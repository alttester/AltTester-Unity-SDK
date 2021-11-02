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

#if ALTUNITYTESTER
            Input.EndTouch(CommandParams.fingerId);
            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}