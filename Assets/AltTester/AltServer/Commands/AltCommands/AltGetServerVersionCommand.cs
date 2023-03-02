using AltTester.AltDriver.Commands;

namespace AltTester.Commands
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
