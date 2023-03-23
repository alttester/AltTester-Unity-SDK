using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
{
    public class AltTapCoordinatesCommand : AltCommandWithWait<AltTapCoordinatesParams, string>
    {
        public AltTapCoordinatesCommand(ICommandHandler handler, AltTapCoordinatesParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.TapCoordinates(CommandParams.coordinates.ToUnity(), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
        }
    }
}