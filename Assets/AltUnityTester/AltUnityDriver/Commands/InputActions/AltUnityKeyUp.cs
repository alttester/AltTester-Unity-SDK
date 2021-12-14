namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityKeyUp : AltBaseCommand
    {
        AltUnityKeyUpParams cmdParams;

        public AltUnityKeyUp(IDriverCommunication commHandler, AltUnityKeyCode keyCode) : base(commHandler)
        {
            this.cmdParams = new AltUnityKeyUpParams(keyCode);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}