using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    class AltUnitySwipeCommand : AltUnityCommandWithWait<AltUnitySwipeParams, string>
    {
        public AltUnitySwipeCommand(ICommandHandler handler, AltUnitySwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
#if ENABLE_INPUT_SYSTEM
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
#if ALTUNITYTESTER
            UnityEngine.Vector2[] positions = { CommandParams.start.ToUnity(), CommandParams.end.ToUnity() };
            Input.SetMultipointSwipe(positions, CommandParams.duration, onFinish);
            return "Ok";


#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
#endif

        }
    }
}
