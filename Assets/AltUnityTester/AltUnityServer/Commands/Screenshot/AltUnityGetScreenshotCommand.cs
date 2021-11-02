using Altom.AltUnityDriver.Commands;
using Altom.AltUnityTester.Communication;
using Altom.AltUnityDriver;

namespace Altom.AltUnityTester.Commands
{
    public class AltUnityGetScreenshotCommand : AltUnityBaseScreenshotCommand<AltUnityGetScreenshotParams, string>
    {
        public AltUnityGetScreenshotCommand(ICommandHandler handler, AltUnityGetScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.StartCoroutine(SendTexturedScreenshotCoroutine(CommandParams.size.ToUnity(), CommandParams.quality));
            return "Ok";
        }
    }
}
