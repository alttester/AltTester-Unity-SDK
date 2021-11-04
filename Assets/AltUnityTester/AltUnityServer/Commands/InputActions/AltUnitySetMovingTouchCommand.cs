using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Altom.AltUnityTester.Commands
{
    class AltUnitySetMultipointSwipeCommand : AltUnityCommand<AltUnityMultipointSwipeParams, string>
    {
        public AltUnitySetMultipointSwipeCommand(AltUnityMultipointSwipeParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            UnityEngine.Vector2[] positions = { CommandParams.start.ToUnity(), CommandParams.end.ToUnity() };
            Input.SetMultipointSwipe(positions, CommandParams.duration);

            return "Ok";
#else
            throw new AltUnityInputModuleException(AltUnityErrors.errorInputModule);
#endif
        }
    }
}
