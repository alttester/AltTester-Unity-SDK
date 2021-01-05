namespace Altom.AltUnityDriver.Commands
{
    public class AltUnityGetServerVersion : AltBaseCommand
    {
        public AltUnityGetServerVersion(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public string Execute()
        {
            string serverVersion;
            SendCommand("getServerVersion");
            serverVersion = Recvall();
            HandleErrors(serverVersion);
            return serverVersion;
        }
    }
}