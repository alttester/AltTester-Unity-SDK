using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
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
