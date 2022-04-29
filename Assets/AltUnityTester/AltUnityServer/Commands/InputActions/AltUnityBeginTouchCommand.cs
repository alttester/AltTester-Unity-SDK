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
#if ALTUNITYTESTER && ENABLE_LEGACY_INPUT_MANAGER
            return Input.BeginTouch(CommandParams.coordinates.ToUnity());
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
