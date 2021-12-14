using Newtonsoft.Json;

namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetTimeScale : AltBaseCommand
    {
        AltUnityGetTimeScaleParams cmdParams;
        public AltUnityGetTimeScale(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltUnityGetTimeScaleParams();
        }
        public float Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<float>(cmdParams);
        }
    }
}