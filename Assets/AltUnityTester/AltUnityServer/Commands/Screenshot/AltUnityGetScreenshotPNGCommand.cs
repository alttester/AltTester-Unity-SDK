using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityGetScreenshotPNGCommand : AltUnityBaseScreenshotCommand<AltUnityGetPNGScreenshotParams, string>
    {
        public AltUnityGetScreenshotPNGCommand(ICommandHandler handler, AltUnityGetPNGScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.StartCoroutine(SendPNGScreenshotCoroutine());
            return "Ok";
        }
    }
}
