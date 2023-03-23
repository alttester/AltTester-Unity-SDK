using System;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
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
