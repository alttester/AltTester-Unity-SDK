using Altom.AltUnityDriver.Logging;
using Altom.Server.Logging;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    // kept for backwards comaptibility. in version <= 1.6.2 this command was enabling extra info to be logged for each commands and logs to be sent to driver.
    // now no extra logs are returned to the driver. Only during errors logs are sent to driver.
    // this method is kept for backwards compatibility and now it reduces the log verbosity in unity.
    // it will be removed in version 1.7.*
    public class AltUnityEnableLoggingCommand : AltUnityCommand
    {
        readonly bool enableLogs;

        public AltUnityEnableLoggingCommand(params string[] parameters) : base(parameters, 3)
        {
            this.enableLogs = bool.Parse(parameters[2]);
        }

        public override string Execute()
        {
            ServerLogManager.SetMinLogLevel(AltUnityLogger.Unity, enableLogs ? AltUnityLogLevel.Debug : AltUnityLogLevel.Info);
            return "Ok";
        }
    }
}
