using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityTiltCommand : AltUnityCommand<AltUnityTiltParams, string>
    {
        public AltUnityTiltCommand(AltUnityTiltParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            Input.Acceleration(CommandParams.acceleration.ToUnity(), CommandParams.duration);
            return "Ok";
#else
            return null;
#endif
        }
    }
}
