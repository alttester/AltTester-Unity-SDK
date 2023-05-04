using System;
using System.Linq;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltMultipointSwipeCommand : AltCommandWithWait<AltMultipointSwipeParams, string>
    {
        public AltMultipointSwipeCommand(ICommandHandler handler, AltMultipointSwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.SetMultipointSwipe(CommandParams.positions.Select(p => p.ToUnity()).ToArray(), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
