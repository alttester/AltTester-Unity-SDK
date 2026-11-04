/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltTapCoordinatesCommand : AltCommandWithWait<AltTapCoordinatesParams, string>
    {
        public AltTapCoordinatesCommand(ICommandHandler handler, AltTapCoordinatesParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.TapCoordinates(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
        }
    }
}
