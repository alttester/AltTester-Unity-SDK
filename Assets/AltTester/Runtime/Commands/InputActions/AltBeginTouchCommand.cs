/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltBeginTouchCommand : AltCommand<AltBeginTouchParams, int>
    {
        public AltBeginTouchCommand(AltBeginTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override int Execute()
        {
            return InputController.BeginTouch(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y));
        }
    }
}
