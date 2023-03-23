using System;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
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
