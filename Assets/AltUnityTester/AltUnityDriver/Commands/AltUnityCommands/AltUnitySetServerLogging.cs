using Altom.AltUnityDriver.Logging;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetServerLogging : AltBaseCommand
    {
        private readonly AltUnitySetServerLoggingParams cmdParams;

        public AltUnitySetServerLogging(IDriverCommunication commHandler, AltUnityLogger logger, AltUnityLogLevel logLevel) : base(commHandler)
        {
            this.cmdParams = new AltUnitySetServerLoggingParams(logger, logLevel);
        }
        public void Execute()
        {
            this.CommHandler.Send(this.cmdParams);
            var data = this.CommHandler.Recvall<string>(this.cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}
