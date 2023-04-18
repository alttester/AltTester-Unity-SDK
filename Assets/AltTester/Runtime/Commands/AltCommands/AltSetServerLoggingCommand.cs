using AltTester.AltTesterUnitySDK.Driver.Commands;
using AltTester.AltTesterUnitySDK.Logging;

namespace AltTester.AltTesterUnitySDK.Commands
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