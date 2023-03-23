using AltTester.AltTesterUnitySdk.Driver;
using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Communication;

namespace AltTester.AltTesterUnitySdk.Commands
{
    public class AltGetScreenshotCommand : AltBaseScreenshotCommand<AltGetScreenshotParams, string>
    {
        public AltGetScreenshotCommand(ICommandHandler handler, AltGetScreenshotParams cmdParams) : base(handler, cmdParams)
        {
        }

        public override string Execute()
        {
            AltRunner._altRunner.StartCoroutine(SendTexturedScreenshotCoroutine(CommandParams.size.ToUnity(), CommandParams.quality));
            return "Ok";
        }
    }
}
