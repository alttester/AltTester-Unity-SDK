namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityEndTouch : AltBaseCommand
    {
        AltUnityEndTouchParams cmdParams;

        public AltUnityEndTouch(IDriverCommunication commHandler, int fingerId) : base(commHandler)
        {
            this.cmdParams = new AltUnityEndTouchParams(fingerId);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}