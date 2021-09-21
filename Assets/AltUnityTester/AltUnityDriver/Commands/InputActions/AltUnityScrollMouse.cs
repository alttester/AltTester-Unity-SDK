namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityScrollMouse : AltBaseCommand
    {
        AltUnityScrollMouseParams cmdParams;
        public AltUnityScrollMouse(IDriverCommunication commHandler, float speed, float duration) : base(commHandler)
        {
            cmdParams = new AltUnityScrollMouseParams(speed, duration);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams).data;
            ValidateResponse("Ok", data);
        }
    }
}