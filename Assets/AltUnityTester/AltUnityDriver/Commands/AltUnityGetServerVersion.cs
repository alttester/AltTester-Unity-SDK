namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetServerVersion : AltBaseCommand
    {
        public AltUnityGetServerVersion(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public string Execute()
        {
            SendCommand("getServerVersion");
            return Recvall();
        }
    }
}