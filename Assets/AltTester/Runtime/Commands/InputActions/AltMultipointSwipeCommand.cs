using System;
using System.Linq;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
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
