using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
{
    class AltTiltCommand : AltCommandWithWait<AltTiltParams, string>
    {
        public AltTiltCommand(ICommandHandler handler, AltTiltParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.Tilt(CommandParams.acceleration.ToUnity(), CommandParams.duration, onFinish);
            return "Ok";
        }

    }
}
