using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;
using Altom.AltUnityDriver;

namespace Assets.AltUnityTester.AltUnityServer.Commands
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
