/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

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
