using AltTester.AltDriver.Commands;
using AltTester.Communication;

namespace AltTester.Commands
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
