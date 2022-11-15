using Newtonsoft.Json;

namespace AltTester.AltDriver.Commands
{
    public class AltGetTimeScale : AltBaseCommand
    {
        AltGetTimeScaleParams cmdParams;
        public AltGetTimeScale(IDriverCommunication commHandler) : base(commHandler)
        {
            cmdParams = new AltGetTimeScaleParams();
        }
        public float Execute()
        {
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<float>(cmdParams);
        }
    }
}