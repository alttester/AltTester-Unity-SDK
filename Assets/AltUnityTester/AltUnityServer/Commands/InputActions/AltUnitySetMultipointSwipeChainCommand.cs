using System.Linq;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnitySetMultipointSwipeChainCommand : AltUnityCommand<AltUnityMultipointSwipeChainParams, string>
    {
        public AltUnitySetMultipointSwipeChainCommand(AltUnityMultipointSwipeChainParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {

#if ALTUNITYTESTER

            Input.SetMultipointSwipe(CommandParams.positions.Select(p => p.ToUnity()).ToArray(), CommandParams.duration);
            return "Ok";
#else
            return null;
#endif
        }
    }
}
