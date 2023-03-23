using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
{
    class AltGetScreenshotPNGCommand : AltBaseScreenshotCommand<AltGetPNGScreenshotParams, string>
    {
        public AltGetScreenshotPNGCommand(ICommandHandler handler, AltGetPNGScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            AltRunner._altRunner.StartCoroutine(SendPNGScreenshotCoroutine());
            return "Ok";
        }
    }
}
