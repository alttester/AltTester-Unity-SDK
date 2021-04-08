using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetServerLogging : AltBaseCommand
    {
        private readonly AltUnityLogger logger;
        private readonly AltUnityLogLevel logLevel;

        public AltUnitySetServerLogging(SocketSettings socketSettings, AltUnityLogger logger, AltUnityLogLevel logLevel) : base(socketSettings)
        {
            this.logger = logger;
            this.logLevel = logLevel;
        }
        public void Execute()
        {
            SendCommand("setServerLogging", logger.ToString(), logLevel.ToString());
            var data = Recvall();

            ValidateResponse("Ok", data);
        }
    }
}
