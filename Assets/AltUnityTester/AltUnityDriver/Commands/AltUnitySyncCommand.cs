namespace Altom.AltUnityDriver.Commands
{
    public class AltUnitySyncCommand : AltBaseCommand
    {
        public AltUnitySyncCommand(SocketSettings socketSettings) : base(socketSettings)
        {
        }
        public void Execute()
        {
            SendCommand("getServerVersion");
            while (true)
            {
                try
                {
                    Recvall();
                    break;
                }
                catch (AltUnityRecvallMessageIdException)
                {
                }
            }
        }
    }
}