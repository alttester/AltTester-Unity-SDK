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
            InputController.SetMultipointSwipe(CommandParams.positions.Select(p => p.ToUnity()).ToArray(), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
