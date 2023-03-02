using System;
using AltTester.AltDriver;
using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
{
    public class AltClickCoordinatesCommand : AltCommandWithWait<AltClickCoordinatesParams, string>
    {
        public AltClickCoordinatesCommand(ICommandHandler handler, AltClickCoordinatesParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }
        public override string Execute()
        {

            InputController.ClickCoordinates(CommandParams.coordinates.ToUnity(), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
        }
    }
}
