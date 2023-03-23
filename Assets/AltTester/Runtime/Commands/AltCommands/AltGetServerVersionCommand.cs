using AltTester.AltTesterUnitySdk.Driver.Commands;

namespace AltTester.AltTesterUnitySdk.Commands
{
    public class AltGetServerVersionCommand : AltCommand<AltGetServerVersionParams, string>
    {
        public AltGetServerVersionCommand(AltGetServerVersionParams cmdParams) : base(cmdParams) { }
        public override string Execute()
        {
            return AltRunner.VERSION;
        }
    }
}
