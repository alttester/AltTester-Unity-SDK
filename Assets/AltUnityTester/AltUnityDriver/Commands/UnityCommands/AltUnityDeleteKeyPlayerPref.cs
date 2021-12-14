namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityDeleteKeyPlayerPref : AltBaseCommand
    {
        AltUnityDeleteKeyPlayerPrefParams cmdParams;
        public AltUnityDeleteKeyPlayerPref(IDriverCommunication commHandler, string keyName) : base(commHandler)
        {
            this.cmdParams = new AltUnityDeleteKeyPlayerPrefParams(keyName);
        }
        public void Execute()
        {
            CommHandler.Send(cmdParams);
            var data = CommHandler.Recvall<string>(cmdParams);
            ValidateResponse("Ok", data);
        }
    }
}