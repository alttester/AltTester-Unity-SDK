using System;
using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltScrollCommand : AltCommandWithWait<AltScrollParams, string>
    {


        public AltScrollCommand(ICommandHandler handler, AltScrollParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.Scroll(CommandParams.speed, CommandParams.speedHorizontal, CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
