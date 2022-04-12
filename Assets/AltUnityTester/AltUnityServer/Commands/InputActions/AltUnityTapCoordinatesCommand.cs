using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityTapCoordinatesCommand : AltUnityCommandWithWait<AltUnityTapCoordinatesParams, string>
    {
        public AltUnityTapCoordinatesCommand(ICommandHandler handler, AltUnityTapCoordinatesParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.TapCoordinates(CommandParams.coordinates.ToUnity(), CommandParams.count, CommandParams.interval, onFinish);
            return "Ok";
        }
    }
}