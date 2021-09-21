using Altom.AltUnityDriver.Commands;
using Altom.Server.Logging;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnitySetServerLoggingCommand : AltUnityCommand<AltUnitySetServerLoggingParams, string>
    {
        public AltUnitySetServerLoggingCommand(AltUnitySetServerLoggingParams cmdParams) : base(cmdParams)
        {
        }

        public override string Execute()
        {
            ServerLogManager.SetMinLogLevel(CommandParams.logger, CommandParams.logLevel);
            return "Ok";
        }
    }
}
