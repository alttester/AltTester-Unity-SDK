namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityKeysUp : AltBaseCommand
    {
        AltUnityKeysUpParams cmdParams;

        public AltUnityKeysUp(IDriverCommunication commHandler, AltUnityKeyCode[] keyCodes) : base(commHandler)
        {
            this.cmdParams = new AltUnityKeysUpParams(keyCodes);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}