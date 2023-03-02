using AltTester.AltDriver.Commands;
using AltTester.Logging;

namespace AltTester.Commands
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
