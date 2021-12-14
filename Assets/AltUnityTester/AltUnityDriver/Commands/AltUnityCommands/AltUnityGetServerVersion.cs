namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetServerVersion : AltBaseCommand
    {
        public AltUnityGetServerVersion(IDriverCommunication commHandler) : base(commHandler)
        {
        }
        public string Execute()
        {
            var cmdParams = new AltUnityGetServerVersionParams();
            CommHandler.Send(cmdParams);
            return CommHandler.Recvall<string>(cmdParams);
        }
    }
}