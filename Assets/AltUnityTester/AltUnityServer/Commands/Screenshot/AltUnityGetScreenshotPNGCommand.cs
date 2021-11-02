using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;

namespace Altom.AltUnityTester.Commands
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
