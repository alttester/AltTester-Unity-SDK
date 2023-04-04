using System;
using AltTester.AltTesterUnitySDK.Driver;
using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Communication;

namespace AltTester.AltTesterUnitySDK.Commands
{
    class AltTiltCommand : AltCommandWithWait<AltTiltParams, string>
    {
        public AltTiltCommand(ICommandHandler handler, AltTiltParams cmdParams) : base(cmdParams, handler, cmdParams.wait)
        {
        }

        public override string Execute()
        {
            InputController.Tilt(CommandParams.acceleration.ToUnity(), CommandParams.duration, onFinish);
            return "Ok";
        }

    }
}
