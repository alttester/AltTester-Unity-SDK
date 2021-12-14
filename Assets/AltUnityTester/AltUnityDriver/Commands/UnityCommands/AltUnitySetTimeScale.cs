using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySetTimeScale : AltBaseCommand
    {
        AltUnitySetTimeScaleParams cmdParams;

        public AltUnitySetTimeScale(IDriverCommunication commHandler, float timeScale) : base(commHandler)
        {
            cmdParams = new AltUnitySetTimeScaleParams(timeScale);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}