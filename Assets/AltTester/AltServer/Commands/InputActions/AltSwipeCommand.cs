using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
{
    class AltSwipeCommand : AltCommandWithWait<AltSwipeParams, string>
    {
        public AltSwipeCommand(ICommandHandler handler, AltSwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {

            UnityEngine.Vector2[] positions = { CommandParams.start.ToUnity(), CommandParams.end.ToUnity() };
            InputController.SetMultipointSwipe(positions, CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
