namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTilt : AltBaseCommand
    {
        AltUnityTiltParams cmdParams;
        public AltUnityTilt(IDriverCommunication commHandler, AltUnityVector3 acceleration, float duration, bool wait) : base(commHandler)
        {
            cmdParams = new AltUnityTiltParams(acceleration, duration, wait);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            string data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
            if (cmdParams.wait)
            {
                data = CommHandler.Recvall<string>(cmdParams);
                ValidateResponse("Finished", data);
            }
        }
    }
}