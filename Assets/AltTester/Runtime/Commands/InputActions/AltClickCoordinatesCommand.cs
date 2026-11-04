/*
    Copyright(C) 2026 Altom Consulting
*/

using AltTester.AltTesterSDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.InputModule;

namespace AltTester.AltTesterUnitySDK.Commands
{
    public class AltClickCoordinatesCommand : AltCommandWithWait<AltClickCoordinatesParams, string>
    {
        public AltClickCoordinatesCommand(ICommandHandler handler, AltClickCoordinatesParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }
        public override string Execute()
        {

            InputController.ClickCoordinates(new UnityEngine.Vector2(CommandParams.coordinates.x, CommandParams.coordinates.y), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
        }
    }
}
