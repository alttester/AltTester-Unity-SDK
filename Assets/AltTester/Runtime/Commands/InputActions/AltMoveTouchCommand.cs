/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltMoveTouchCommand : AltCommand<AltMoveTouchParams, string>
    {
        public AltMoveTouchCommand(AltMoveTouchParams cmdParams) : base(cmdParams)
        {

        }
        public override string Execute()
        {
            InputController.MoveTouch(CommandParams.fingerId, new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y));
            return "Ok";

        }
    }
}
