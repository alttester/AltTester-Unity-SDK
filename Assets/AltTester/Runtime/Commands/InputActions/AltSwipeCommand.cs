/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltSwipeCommand : AltCommandWithWait<AltSwipeParams, string>
    {
        public AltSwipeCommand(ICommandHandler handler, AltSwipeParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {

            UnityEngine.Vector2[] positions = { new UnityEngine.Vector2(CommandParams.start.x, CommandParams.start.y), new UnityEngine.Vector2(CommandParams.end.x, CommandParams.end.y) };
            InputController.SetMultipointSwipe(positions, CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
