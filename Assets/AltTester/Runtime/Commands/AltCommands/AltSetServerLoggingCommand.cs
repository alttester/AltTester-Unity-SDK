using AltTester.AltTesterUnitySdk.Driver.Commands;
using AltTester.AltTesterUnitySdk.Logging;

namespace AltTester.AltTesterUnitySdk.Commands
{
    public class AltSetServerLoggingCommand : AltCommand<AltSetServerLoggingParams, string>
    {
        public AltSetServerLoggingCommand(AltSetServerLoggingParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            ServerLogManager.SetMinLogLevel(CommandParams.logger, CommandParams.logLevel);
            return "Ok";
        }
    }
}
