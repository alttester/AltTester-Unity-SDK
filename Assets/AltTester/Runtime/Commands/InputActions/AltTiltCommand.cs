/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver;
using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltTiltCommand : AltCommandWithWait<AltTiltParams, string>
    {
        public AltTiltCommand(ICommandHandler handler, AltTiltParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.Tilt(new UnityEngine.Vector2(CommandParams.acceleration.x, CommandParams.acceleration.y), CommandParams.duration, onFinish);
            return "Ok";
        }

    }
}
