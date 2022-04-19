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
#if ENABLE_INPUT_SYSTEM
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
#if ALTUNITYTESTER
            Input.MoveTouch(CommandParams.fingerId, CommandParams.coordinates.ToUnity());

#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
#endif
            return "Ok";

        }
    }
}