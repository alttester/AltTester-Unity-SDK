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
#if ENABLE_INPUT_SYSTEM
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
#if ALTUNITYTESTER
            Input.EndTouch(CommandParams.fingerId);

#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
#endif
            return "Ok";

        }
    }
}