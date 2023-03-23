using System;
using System.Linq;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
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
