namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDeletePlayerPref : AltBaseCommand
    {
        AltUnityDeletePlayerPrefParams cmdParams;
        public AltUnityDeletePlayerPref(IDriverCommunication commHandler) : base(commHandler)
        {
            this.cmdParams = new AltUnityDeletePlayerPrefParams();
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}