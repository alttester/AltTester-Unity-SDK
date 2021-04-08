using System;
using Altom.AltUnityDriver.Logging;
using Altom.Server.Logging;
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnitySetServerLoggingCommand : AltUnityCommand
    {
        readonly AltUnityLogger logger;
        readonly AltUnityLogLevel logLevel;

        public AltUnitySetServerLoggingCommand(params string[] parameters) : base(parameters, 4)
        {
            logger = (AltUnityLogger)Enum.Parse(typeof(AltUnityLogger), parameters[2]);
            logLevel = (AltUnityLogLevel)Enum.Parse(typeof(AltUnityLogLevel), parameters[3]);
        }

        public override string Execute()
        {
            ServerLogManager.SetMinLogLevel(logger, logLevel);
            return "Ok";
        }
    }
}
