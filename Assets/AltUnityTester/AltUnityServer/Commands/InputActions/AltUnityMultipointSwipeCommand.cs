using System;
using System.Linq;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityMultipointSwipeCommand : AltUnityCommandWithWait<AltUnityMultipointSwipeParams, string>
    {
        public AltUnityMultipointSwipeCommand(ICommandHandler handler, AltUnityMultipointSwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
#if ENABLE_INPUT_SYSTEM
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
#if ALTUNITYTESTER

            Input.SetMultipointSwipe(CommandParams.positions.Select(p => p.ToUnity()).ToArray(), CommandParams.duration, onFinish);

#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
#endif
            return "Ok";
        }
    }
}
