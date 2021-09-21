namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityTilt : AltBaseCommand
    {
        AltUnityTiltParams cmdParams;
        public AltUnityTilt(IDriverCommunication commHandler, AltUnityVector3 acceleration, float duration) : base(commHandler)
        {
            cmdParams = new AltUnityTiltParams(acceleration, duration);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            string data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}