/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltMoveMouseCommand : AltCommandWithWait<AltMoveMouseParams, string>
    {
        public AltMoveMouseCommand(ICommandHandler handler, AltMoveMouseParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.MoveMouse(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y), CommandParams.duration, onFinish);
            return "Ok";
        }
    }
}
